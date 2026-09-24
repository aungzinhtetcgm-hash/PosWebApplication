using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Controllers;
using PosWebApplication.DTOs.Comment;
using PosWebApplication.DTOs.Post;
using PosWebApplication.Entity;
using PosWebApplication.Exceptions;
using PosWebApplication.Services.Comment;
using PosWebApplication.Services.Post;
using PosWebApplication.ViewModels.Comment;
using PosWebApplication.ViewModels.Post;
using System.Security.Claims;

namespace PosWebApplication.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IValidator<CreatePostDTO> _createValidator;
        private readonly IValidator<UpdatePostDTO> _updateValidator;
        private readonly ICommentService _commentService;
        private readonly IValidator<CreateCommentDTO> _commentValidator;

        public PostController(
            IPostService postService,
            IValidator<CreatePostDTO> createValidator,
            IValidator<UpdatePostDTO> updateValidator,
            ICommentService commentService,
            IValidator<CreateCommentDTO> commentValidator)
        {
            _postService = postService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _commentService = commentService;
            _commentValidator = commentValidator;
        }

        // Index

        [HttpGet]
        public IActionResult Index()
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            var model = new PostIndexViewModel();

            var publicPosts =
                _postService.GetPublicPosts();

            model.PublicPosts = publicPosts
                .Where(x => x.created_by == u_id.Value)
                .Select(x => new PublicPostViewModel
                {
                    p_id = x.p_id,
                    title = x.title,
                    description = x.description,
                    name = x.user?.name,
                    created_at = x.created_at
                })
                .ToList();

            var myPosts =
                _postService.GetMyPosts(
                    u_id.Value);

            model.PrivatePosts = myPosts
                .Where(x => x.public_flag == "private")
                .Select(x => new PrivatePostViewModel
                {
                    p_id = x.p_id,
                    title = x.title,
                    description = x.description,
                    created_at = x.created_at,
                    updated_at = x.updated_at
                })
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            CreatePostViewModel model)
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            var dto = new CreatePostDTO
            {
                title = model.title,
                description = model.description,
                public_flag = model.public_flag
            };

            var validationResult =
                _createValidator.Validate(dto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(
                        error.PropertyName,
                        error.ErrorMessage);
                }

                return View(model);
            }

            _postService.Create(
                dto,
                u_id.Value);

            return RedirectToAction(
                nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            var post = _postService
                .GetMyPosts(u_id.Value)
                .FirstOrDefault(x => x.p_id == id);

            if (post == null)
            {
                throw new NotFoundException(
           "Post not found.");
            }

            var model = new EditPostViewModel
            {
                p_id = post.p_id,
                title = post.title,
                description = post.description,
                public_flag = post.public_flag
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            EditPostViewModel model)
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            var dto = new UpdatePostDTO
            {
                p_id = model.p_id,
                title = model.title,
                description = model.description,
                public_flag = model.public_flag
            };

            var validationResult =
                _updateValidator.Validate(dto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(
                        error.PropertyName,
                        error.ErrorMessage);
                }

                return View(model);
            }

            try
            {
                _postService.Update(
                    dto,
                    u_id.Value);

                return RedirectToAction(
                    nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            try
            {
                _postService.Delete(
                    id,
                    u_id.Value);

                return RedirectToAction(
                    nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            var post = _postService.GetPost(
                id,
                u_id.Value);

            if (post == null)
            {
                throw new NotFoundException(
                    "Post not found.");
            }

            var comments =
                _commentService.GetByPostId(id);

            var model = new PostDetailsViewModel
            {
                p_id = post.p_id,
                title = post.title,
                description = post.description,
                public_flag = post.public_flag,
                name = post.user?.name,
                created_at = post.created_at,
                updated_at = post.updated_at,

                Comments = comments
                    .Select(x => new CommentViewModel
                    {
                        c_id = x.c_id,
                        u_id = x.u_id,
                        name = x.user?.name,
                        comments = x.comments,
                        created_at = x.created_at
                    })
                    .ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(
            CreateCommentDTO model)
        {
            var u_id = GetCurrentu_id();

            if (u_id == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            var validationResult =
                _commentValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(
                        error.PropertyName,
                        error.ErrorMessage);
                }

                return RedirectToAction(
                    nameof(Details),
                    new { id = model.p_id });
            }

            var post = _postService.GetPost(
                model.p_id,
                u_id.Value);

            if (post == null)
            {
                return NotFound();
            }

            if (post.public_flag != "public")
            {
                return Forbid();
            }

            _commentService.Create(
                model,
                u_id.Value);

            return RedirectToAction(
                nameof(Details),
                new { id = model.p_id });
        }

        private int? GetCurrentu_id()
        {
            var claim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return null;
            }

            if (!int.TryParse(
                claim.Value,
                out var u_id))
            {
                return null;
            }

            return u_id;
        }
    }
}