using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace PosWebApplication.Attributes
{
    public class Currentu_idAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var u_id =
                context.HttpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(u_id, out var parsedu_id))
            {
                context.Result = new UnauthorizedObjectResult(
                    new
                    {
                        success = false,
                        message = "User ID not found in JWT."
                    });

                return;
            }

            context.HttpContext.Items["u_id"] =
                parsedu_id;
        }
    }
}