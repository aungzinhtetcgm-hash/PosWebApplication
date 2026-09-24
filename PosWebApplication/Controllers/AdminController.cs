using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApplication.DAO.UserDAO;
using PosWebApplication.Services.Admin;
using PosWebApplication.ViewModels.Admin;

namespace PosWebApplication.Controllers
{
    [Authorize(Roles = "1")]
    public class AdminController : BaseController
    {
        private readonly IUserDAO _userDAO;
        private readonly IAdminService _adminService;

        public AdminController(
            IUserDAO userDAO,
            IAdminService adminService)
        {
            _userDAO = userDAO;
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var users = _userDAO.GetAll();

            var viewModel = new AdminDashboardViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MakeAdmin(int u_id)
        {
            var result = _adminService.MakeAdmin(u_id);

            if (!result)
            {
                WarningMessage(
                    "User could not be updated.");

                return RedirectToAction(
                    nameof(Dashboard));
            }

            SuccessMessage(
                "User has been updated to Admin.");

            return RedirectToAction(
                nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int u_id)
        {
            var result = _adminService.DeleteUser(u_id);

            if (!result)
            {
                WarningMessage(
                    "User could not be deleted.");

                return RedirectToAction(
                    nameof(Dashboard));
            }

            SuccessMessage(
                "User deleted successfully.");

            return RedirectToAction(
                nameof(Dashboard));
        }
    }
}