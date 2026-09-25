using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Models;
using PosWebApplication.Services.Post;
using PosWebApplication.ViewModels.Post;
using System.Diagnostics;
using PosWebApplication.Models;
using PosWebApplication.Services.Post;
using PosWebApplication.ViewModels.Post;

namespace PosWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;

        public HomeController(
            ILogger<HomeController> logger,
            IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var publicPosts =
                _postService.GetPublicPosts();

            var model = publicPosts
                .Select(x => new PublicPostViewModel
                {
                    p_id = x.p_id,
                    title = x.title,
                    description = x.description,
                    name = x.user?.name,
                    created_at = x.created_at
                })
                .ToList();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                });
        }

        public IActionResult TestError()
        {
            throw new Exception(
                "This is a test error.");
        }
    }
}