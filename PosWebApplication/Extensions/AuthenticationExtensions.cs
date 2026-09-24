//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace PosWebApplication.Extensions
//{
//    public static class AuthenticationExtensions
//    {
//        public static IServiceCollection ConfigurePosWebApplicationAuthentication(
//            this IServiceCollection services,
//            IConfiguration configuration 
//            )
//        {
//            services.AddAuthentication(options =>
//            {
//                options.DefaultScheme =
//                CookieAuthenticationDefaults.AuthenticationScheme;

//                options.DefaultAuthenticateScheme =
//                CookieAuthenticationDefaults.AuthenticationScheme;

//                options.DefaultChallengeScheme =
//                CookieAuthenticationDefaults.AuthenticationScheme;

//                options.DefaultSignInScheme =
//                CookieAuthenticationDefaults.AuthenticationScheme;
//            }
//            )

//                .AddCookie(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    options =>
//                    {
//                        options.LoginPath = "/User/Login";

//                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

//                        options.SlidingExpiration = true;

//                        options.AccessDeniedPath =
//                        "/User/AccessDenied";

//                        options.Cookie.SameSite =
//                        Microsoft.AspNetCore.Http.SameSiteMode.Lax;

//                        options.Cookie.SecurePolicy =
//                        Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
//                    })

//                .AddJwtBearer(
//                    JwtBearerDefaults.AuthenticationScheme,
//                    options =>
//                    {
//                        options.TokenValidationParameters =
//                        new TokenValidationParameters
//                        {
//                            ValidateIssuerSigningKey = true,
//                            ValidateIssuer = true,
//                            ValidateAudience = true,
//                            ValidateLifetime = true,

//                            ClockSkew = TimeSpan.Zero,

//                            ValidIssuer =
//                                configuration["JwtSettings:Issuer"],

//                            ValidAudience =
//                                configuration["JwtSettings:Audience"],

//                            IssuerSigningKey =
//                                new SymmetricSecurityKey(
//                                    Encoding.UTF8.GetBytes(
//                                        configuration[
//                                            "JwtSettings:Key"
//                                            ]
//                                            ?? string.Empty
//                                        ))
//                        };

//                    }
//                );

//            return services;
//        }

//        public static IServiceCollection ConfigurePosWebApplicationAuthorization(
//            this IServiceCollection services
//            )
//        {
//            services.AddAuthorization(options =>
//            {
//                options.AddPolicy(
//                    "WebPolicy",
//                    policy =>
//                    {
//                        policy.RequireAuthenticatedUser();
//                        policy.AuthenticationSchemes.Add(
//                            CookieAuthenticationDefaults
//                            .AuthenticationScheme);
//                    });
//                options.AddPolicy(
//                                   "ApiPolicy",
//                                   policy =>
//                                   {
//                                       policy.RequireAuthenticatedUser();

//                                       policy.AuthenticationSchemes.Add(
//                                           JwtBearerDefaults
//                                               .AuthenticationScheme);
//                                   });
//            });

//            return services;
//        }



//    }
//}



using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace PosWebApplication.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigurePosWebApplicationAuthentication(
            this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults
                        .AuthenticationScheme;

                options.DefaultSignInScheme =
                    CookieAuthenticationDefaults
                        .AuthenticationScheme;

                options.DefaultChallengeScheme =
                    CookieAuthenticationDefaults
                        .AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login";

                options.AccessDeniedPath =
                    "/User/AccessDenied";

                options.ExpireTimeSpan =
                    TimeSpan.FromMinutes(30);

                options.SlidingExpiration = true;

                });

            services.AddAuthorization();

            return services;
        }
    }
}