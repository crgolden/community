using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using community.Api.v1.Controllers;
using community.Core.Interfaces;
using community.Core.Models;
using community.Core.Services;
using community.Data;
using community.Data.Managers;
using community.Extensions;

namespace community
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext(
               windowsConnectionString: Configuration.GetConnectionString("WindowsConnection"),
               macOsConnectionString: Configuration.GetConnectionString("macOsConnection"));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IManager<Event>, EventManager>();
            services.AddScoped<IManager<Address>, AddressManager>();

            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            services.AddAuthentication(Configuration.GetSection(nameof(JwtOptions)));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("User"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                .ConfigureApplicationPartManager(apm =>
                {
                    apm.ApplicationParts.Add(new AssemblyPart(typeof(AccountController).GetTypeInfo().Assembly));
                    apm.ApplicationParts.Add(new AssemblyPart(typeof(AddressesController).GetTypeInfo().Assembly));
                    apm.ApplicationParts.Add(new AssemblyPart(typeof(EventsController).GetTypeInfo().Assembly));
                    apm.ApplicationParts.Add(new AssemblyPart(typeof(ManageController).GetTypeInfo().Assembly));
                    apm.ApplicationParts.Add(new AssemblyPart(typeof(UsersController).GetTypeInfo().Assembly));
                });

            services.AddSwaggerDocumentation();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ApplicationDbContext context,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseCors();

            app.UseSwaggerDocumentation();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {controller = "Home", action = "Index"});
            });

            var seed = new Seed(context, userManager, roleManager);
            string adminEmail = Configuration.GetSection("AdminEmail").Value,
                adminPassword = Configuration.GetSection("AdminPassword").Value;
            Task.Run(() => seed.SeedData(adminEmail, adminPassword)).Wait();
        }

    }
}
