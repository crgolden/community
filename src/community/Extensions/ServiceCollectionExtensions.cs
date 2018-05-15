using System;
using System.Runtime.InteropServices;
using System.Text;
using community.Core.Models;
using community.Data;
using community.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace community.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Community API", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Type = "apiKey",
                    In = "Header",
                    Name = "Authorization",
                    Description = "Input \"Bearer {token}\" (without quotes)"
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services,
            string windowsConnectionString, string macOsConnectionString)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(windowsConnectionString,
                        x => x.MigrationsAssembly("community.Data")));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(macOsConnectionString,
                        x => x.MigrationsAssembly("community.Data")));

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services,
            IConfigurationSection jwtOptions)
        {
            string
                secretKey = jwtOptions[nameof(JwtOptions.SecretKey)],
                issuer = jwtOptions[nameof(JwtOptions.Issuer)],
                audience = jwtOptions[nameof(JwtOptions.Audience)];

            services
                .Configure<JwtOptions>(options =>
                {
                    options.Issuer = issuer;
                    options.Audience = audience;
                    options.SecretKey = secretKey;
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = audience;
                    options.ClaimsIssuer = issuer;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                });

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:12345"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options => { options.AddPolicy("<YourCorsPolicyName>", corsBuilder.Build()); });

            return services;
        }
    }
}
