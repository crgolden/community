using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using community.Data;
using community.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using community.Services;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.IdentityModel.Tokens;

namespace community
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

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure tokens
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("User"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
           ILoggerFactory loggerFactory, ApplicationDbContext context,
           UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
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

            var seed = new Seed(context, userManager, roleManager);
            string adminEmail = Configuration.GetSection("AdminEmail").Value,
                adminPassword = Configuration.GetSection("AdminPassword").Value;
            Task.Run(() => seed.SeedData(adminEmail, adminPassword)).Wait();
        }
    }
}