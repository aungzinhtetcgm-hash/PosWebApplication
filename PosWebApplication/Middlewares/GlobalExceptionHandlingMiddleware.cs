using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PosWebApplication.Exceptions;

namespace PosWebApplication.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _contextAccessor;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            IHttpContextAccessor contextAccessor)
        {
            _next = next;
            _contextAccessor = contextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (context.Request.Path.Value?
                    .StartsWith("/api", StringComparison.OrdinalIgnoreCase) == true)
                {
                    await ExceptionHandlingAsync(context, exception);
                }
                else
                {
                    await ExceptionErrorPageHandlingAsync(context, exception);
                }
            }
        }

        private async Task ExceptionErrorPageHandlingAsync(
            HttpContext context,
            Exception exception)
        {
            string url = context.Request.Path.Value ?? string.Empty;
            string method = context.Request.Method;

            string errorView;
            int statusCode;
            string pageTitle;

            switch (exception)
            {
                case UnauthorizedException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    pageTitle = "Unauthorized Access";
                    errorView = "~/Views/Errors/401.cshtml";
                    break;

                case UnauthorizedAdminException:
                    statusCode = StatusCodes.Status403Forbidden;
                    pageTitle = "Access Denied";
                    errorView = "~/Views/Errors/403.cshtml";
                    break;

                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    pageTitle = "Data Not Found";
                    errorView = "~/Views/Errors/404.cshtml";
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    pageTitle = "Internal Server Error";
                    errorView = "~/Views/Errors/500.cshtml";
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "text/html";

            var html = await RenderViewToString(
                context,
                errorView,
                pageTitle);

            await context.Response.WriteAsync(html);
        }

        private async Task ExceptionHandlingAsync(
            HttpContext context,
            Exception exception)
        {
            string url = context.Request.Path.Value ?? string.Empty;
            string method = context.Request.Method;

            var errorDetail = new ErrorDetail
            {
                status = StatusCodes.Status500InternalServerError,
                title = "Internal Server Error",
                detail = "An unexpected error occurred. Please try again later."
            };

            switch (exception)
            {
                case UnauthorizedException:
                    errorDetail.status =
                        StatusCodes.Status401Unauthorized;

                    errorDetail.title = "Unauthorized";

                    errorDetail.detail = exception.Message;
                    break;

                case UnauthorizedAdminException:
                    errorDetail.status =
                        StatusCodes.Status403Forbidden;

                    errorDetail.title = "Forbidden";

                    errorDetail.detail = exception.Message;
                    break;

                case NotFoundException:
                    errorDetail.status =
                        StatusCodes.Status404NotFound;

                    errorDetail.title = "Not Found";

                    errorDetail.detail = exception.Message;
                    break;
            }

            context.Response.StatusCode = errorDetail.status;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(errorDetail));
        }

        private async Task<string> RenderViewToString(
            HttpContext context,
            string viewPath,
            string pageTitle = "")
        {
            var actionContext = new ActionContext(
                context,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            var serviceProvider = context.RequestServices;

            var viewEngine =
                serviceProvider
                    .GetRequiredService<
                        Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine>();

            var tempDataProvider =
                serviceProvider
                    .GetRequiredService<
                        Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>();

            using var sw = new StringWriter();

            var viewResult = viewEngine.GetView(
                null,
                viewPath,
                false);

            if (!viewResult.Success)
            {
                throw new FileNotFoundException(
                    $"View {viewPath} not found.");
            }

            var viewDictionary =
                new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    Model = null
                };

            viewDictionary["Title"] = pageTitle;

            var viewContext =
                new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                        context,
                        tempDataProvider),
                    sw,
                    new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }

        private class ErrorDetail
        {
            public int status { get; set; }

            public string title { get; set; }

            public string detail { get; set; }
        }
    }
}