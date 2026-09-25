using Microsoft.AspNetCore.Mvc;
using PosWebApplication.DTOs.User;
using PosWebApplication.Services.User;
using System.Net;
using PosWebApplication.Controllers.Api;
using PosWebApplication.DTOs.User;
using PosWebApplication.Services.User;

[Route("api/user")]
public class UserApiController : BaseApiController
{
    private readonly IUserService _userService;

    public UserApiController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register(
        [FromBody] RegisterDTO model)
    {
        var result = _userService.Register(model);

        if (!result)
        {
            return SendError(
                "Email already exists.",
                null,
                HttpStatusCode.Conflict);
        }

        return SendResponse<object?>(
            null,
            "User registered successfully.",
            HttpStatusCode.Created);
    }
}