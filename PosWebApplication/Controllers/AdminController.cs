using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Constraints;
using PosWebApplication.DAO.UserDAO;
using PosWebApplication.Services;
using PosWebApplication.Services.Admin;
using PosWebApplication.ViewModels.Admin;

namespace PosWebApplication.Controller
{
    public class AdminController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUserDAO _userDAO;
        private readonly IAdminService _adminService;
        private readonly SessionService _sessionService;

        public AdminController(
            IUserDAO userDAO,
            IAdminService adminService,
            SessionService sessionService)
        {
            _userDAO = userDAO;
            _adminService = adminService;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var currentUser = _sessionService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (currentUser.role != UserRoles.Admin)
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _userDAO.GetAll();

            var viewModel = new AdminDashboardViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult MakeAdmin(int userId)
        {
            var currentUser = _sessionService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (currentUser.role != UserRoles.Admin)
            {
                return RedirectToAction("Index", "Home");
            }

            _adminService.MakeAdmin(userId);

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            var currentUser = _sessionService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (currentUser.role != UserRoles.Admin)
            {
                return RedirectToAction("Index", "Home");
            }

            _adminService.DeleteUser(userId);

            return RedirectToAction("Dashboard");
        }
    }
}
