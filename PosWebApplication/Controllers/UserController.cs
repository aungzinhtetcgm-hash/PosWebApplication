using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Constraints;
using PosWebApplication.DTOs.User;
using PosWebApplication.Entity;
using PosWebApplication.Helper;
using PosWebApplication.Services;
using PosWebApplication.Services.User;

namespace PosWebApplication.Controllers
{
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUserService _userService;
        private readonly SessionService _sessionService;
        private readonly FilePathHelper _filePathHelper;

        public UserController(
            IUserService userService,
            SessionService sessionService,
            FilePathHelper filePathHelper)
        {
            _userService = userService;
            _sessionService = sessionService;
            _filePathHelper = filePathHelper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.Login(model);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password.");

                return View(model);
            }

            _sessionService.SetUser(user);

            if (user.role == UserRoles.Admin)
            {
                return RedirectToAction(
                    "Dashboard",
                    "Admin");
            }

            return RedirectToAction(
                "UserDashboard",
                "User");
        }


        [HttpGet]
        public IActionResult UserDashboard()
        {
            var currentUser =
                _sessionService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var currentUser =
                _sessionService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(user model, IFormFile? image)
        {
            var currentUser =
                _sessionService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login");
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

                if (!allowedExtensions.Contains(extension))
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
                    Directory.CreateDirectory(uploadPath);
                }

                string fileName =
                    currentUser.name + extension;

                string filePath =
                    Path.Combine(
                        uploadPath,
                        fileName
                    );

                using (var stream =
                    new FileStream(
                        filePath,
                        FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                currentUser.img = fileName;

                _userService.UpdateProfile(currentUser);

                _sessionService.SetUser(currentUser);
            }

            return RedirectToAction("UserDashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _sessionService.Clear();

            return RedirectToAction("Login");
        }
    }
}