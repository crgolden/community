using System.Runtime.InteropServices;
using Community.Data;
using Community.Data.Seed;
using Community.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Community.Services;
using Microsoft.AspNetCore.SpaServices.Webpack;

namespace Community
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            string connectionString;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                connectionString = Configuration.GetConnectionString("WindowsConnection");

                services.AddDbContext<ApplicationDbContext>(x =>
                    x.UseSqlServer(connectionString));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                connectionString = Configuration.GetConnectionString("OSXConnection");

                services.AddDbContext<ApplicationDbContext>(x =>
                    x.UseSqlite(connectionString));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy =>
                    policy.RequireClaim("User"));
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim("Admin"));
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
           ILoggerFactory loggerFactory, ApplicationDbContext context,
           UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
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

            app.UseMvc(x =>
            {
                x.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                x.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            context.Database.Migrate();

            var seed = new Seed(context, Configuration["AdminEmail"], Configuration["AdminPassword"],
                userManager, roleManager);

            Task.Run(seed.SeedData).Wait();
        }
    }
}
