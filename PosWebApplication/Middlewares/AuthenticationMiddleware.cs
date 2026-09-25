using PosWebApplication.Services;

namespace PosWebApplicatioin.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            SessionService sessionService)
        {
            var path = context.Request.Path.Value?.ToLower();

            var publicPaths = new[]
            {
                "/",
                "/home/index",
                "/home/privacy",
                "/user/login",
                "/user/register",
                "/post/details/p_id"
            };

            var isPublicPath = publicPaths.Any(x =>
                path == x ||
                path?.StartsWith(x + "/") == true
            );

            if (!isPublicPath)
            {
                var currentUser = sessionService.GetUser();

                if (currentUser == null)
                {
                    context.Response.Redirect("/User/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}