using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Constraints;
using PosWebApplication.Controllers;
using PosWebApplication.DTOs.User;
using PosWebApplication.Entity;
using PosWebApplication.Helper;
using PosWebApplication.Services.User;
using System.Security.Claims;

namespace PosWebApplication.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly FilePathHelper _filePathHelper;

        public UserController(
            IUserService userService,
            FilePathHelper filePathHelper)
        {
            _userService = userService;
            _filePathHelper = filePathHelper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _userService.Register(model);

            if (!result)
            {
                ModelState.AddModelError(
                    "Email",
                    "This email is already registered."
                );

                return View(model);
            }

            SuccessMessage(
                "Registration successful.");

            return RedirectToAction(
                nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.Login(model);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.u_id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.name ?? string.Empty),

                new Claim(
                    ClaimTypes.Email,
                    user.email ?? string.Empty),

                new Claim(
                    ClaimTypes.Role,
                    user.role.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults
                    .AuthenticationScheme);

            var principal = new ClaimsPrincipal(
                identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                principal);

            if (user.role == UserRoles.Admin)
            {
                return RedirectToAction(
                    "Dashboard",
                    "Admin");
            }

            return RedirectToAction(
                nameof(UserDashboard));
        }

        // User Dashboard

        [Authorize]
        [HttpGet]
        public IActionResult UserDashboard()
        {
            return View();
        }

        // Profile

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction(
                    nameof(Login));
            }

            var currentUser =
                _userService.GetById(userId.Value);

            if (currentUser == null)
            {
                return NotFound();
            }

            return View(currentUser);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(
            user model,
            IFormFile? image)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction(
                    nameof(Login));
            }

            var currentUser =
                _userService.GetById(userId.Value);

            if (currentUser == null)
            {
                return NotFound();
            }

            if (image != null && image.Length > 0)
            {
                const long maxFileSize =
                    2 * 1024 * 1024;

                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

                string extension =
                    Path.GetExtension(image.FileName)
                        .ToLowerInvariant();

                if (!allowedExtensions.Contains(
                    extension))
                {
                    ModelState.AddModelError(
                        "image",
                        "Only JPG, JPEG and PNG images are allowed."
                    );

                    return View(currentUser);
                }

                if (image.Length > maxFileSize)
                {
                    ModelState.AddModelError(
                        "image",
                        "Image size must not exceed 2 MB."
                    );

                    return View(currentUser);
                }

                string uploadPath =
                    _filePathHelper.GetUserImagePath();

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(
                        uploadPath);
                }

                string fileName =
                    currentUser.name + extension;

                string filePath =
                    Path.Combine(
                        uploadPath,
                        fileName);

                await using (
                    var stream = new FileStream(
                        filePath,
                        FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                currentUser.img = fileName;

                _userService.UpdateProfile(
                    currentUser);
            }

            SuccessMessage(
                "Profile updated successfully.");

            return RedirectToAction(
                nameof(UserDashboard));
        }

        // Logout

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme);

            SuccessMessage(
                "Logout successful.");

            return RedirectToAction(
                nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return null;
            }

            if (!int.TryParse(
                claim.Value,
                out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}