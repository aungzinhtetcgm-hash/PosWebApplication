using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PosWebApplication.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IActionResult SendErrorResponse(
            HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewData["Is404"] = true;

                return View(
                    "~/Views/Errors/404.cshtml");
            }

            return RedirectToAction(
                "Login",
                "User");
        }

        protected void SuccessMessage(
            string message,
            string title = "Info")
        {
            TempData["ShowModal"] = true;
            TempData["ModalTitle"] = title;
            TempData["ModalIcon"] =
                "fa-check-circle text-success";
            TempData["PosWebApplication"] = message;
        }

        protected void WarningMessage(
            string message,
            string title = "Warning")
        {
            TempData["ShowModal"] = true;
            TempData["ModalTitle"] = title;
            TempData["ModalIcon"] =
                "fa-exclamation-triangle text-warning";
            TempData["PosWebApplication"] = message;
        }

        protected void ErrorMessage(
            string message,
            string title = "Error")
        {
            TempData["ShowModal"] = true;
            TempData["ModalTitle"] = title;
            TempData["ModalIcon"] =
                "fa-times-circle text-danger";
            TempData["PosWebApplication"] = message;
        }
    }
}