using System.Runtime.InteropServices;
using Community.Data;
using Community.Data.Seed;
using Community.Models;
using Community.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Community
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WindowsConnection")));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("OSXConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy =>
                    policy.RequireClaim("Permission", RoleClaims.ValuesUser));
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim("Permission", RoleClaims.ValuesAdmin));
            });
            services.AddMvc();
            services.AddTransient<IEmailSender, AuthMessageSender>()
                .AddTransient<ISmsSender, AuthMessageSender>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage()
                    .UseBrowserLink();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles()
                .UseIdentity()
                .UseMvcWithDefaultRoute();

            new Seed(context, Configuration).SeedData().Wait();
        }
    }
}