using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PosWebApplication.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult SendResponse<T>(

                T result,
                string message,
                HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new
            {
                success = true,
                message = message,
                data = result
            };

            return StatusCode(
                (int)statusCode,
                response);
        }

        protected IActionResult SendError(
            string error,
            object? errorMessages = null,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var response = new
            {
                success = false,
                message = error,
                errors = errorMessages
            };

            return StatusCode(
                (int)statusCode,
                response);
        }
    }
}
