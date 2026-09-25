using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PosWebApplication.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection ConfigurePosWebApplicationAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtKey = configuration["JWT:Key"];
            var jwtIssuer = configuration["JWT:Issuer"];
            var jwtAudience = configuration["JWT:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "JWT:Key is missing in appsettings.json.");
            }

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new InvalidOperationException(
                    "JWT:Issuer is missing in appsettings.json.");
            }

            if (string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new InvalidOperationException(
                    "JWT:Audience is missing in appsettings.json.");
            }

            services.AddAuthentication(options =>
            {
                // Website အတွက် default = Cookie
                options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultSignInScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/AccessDenied";

                    options.ExpireTimeSpan =
                        TimeSpan.FromMinutes(30);

                    options.SlidingExpiration = true;

                    options.Cookie.HttpOnly = true;

                    options.Cookie.SameSite =
                        Microsoft.AspNetCore.Http.SameSiteMode.Lax;

                    options.Cookie.SecurePolicy =
                        Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                })
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtKey)),

                            ValidateIssuer = true,
                            ValidIssuer = jwtIssuer,

                            ValidateAudience = true,
                            ValidAudience = jwtAudience,

                            ValidateLifetime = true,

                            ClockSkew = TimeSpan.Zero
                        };
                });

            services.AddAuthorization();

            return services;
        }
    }
}