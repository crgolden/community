using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using community.Api.v1.Controllers;
using community.Core.Interfaces;
using community.Core.Models;
using community.Core.Services;
using community.Data;
using community.Data.Managers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

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
            SetDbContext(services);

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IManager<Event>, EventManager>();
            services.AddScoped<IManager<Address>, AddressManager>();

            SetAuthentication(services);

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

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Community API", Version = "v1"}); });
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

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Community API V1"); });

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

        private void SetDbContext(IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WindowsConnection"),
                        x => x.MigrationsAssembly("community.Data")));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("macOSConnection"),
                        x => x.MigrationsAssembly("community.Data")));
        }

        private void SetAuthentication(IServiceCollection services)
        {
            var tokenOptions = Configuration
                .GetSection("TokenProviderOptions")
                .GetChildren()
                .ToList();
            string
                secretKey = tokenOptions.Single(x => x.Key.Equals("SecretKey")).Value,
                issuer = tokenOptions.Single(x => x.Key.Equals("Issuer")).Value,
                audience = tokenOptions.Single(x => x.Key.Equals("Audience")).Value;
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var signingKey = new SymmetricSecurityKey(secretKeyBytes);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Audience = audience;
                    options.ClaimsIssuer = issuer;
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                });
        }
    }
}
