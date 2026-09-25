using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Controllers.Api;
using PosWebApplication.DTOs.Post;
using PosWebApplication.Services.Post;
using System.Net;
using PosWebApplication.Attributes;
using PosWebApplication.DTOs.Post;
using PosWebApplication.Services.Post;

namespace PosWebApplication.Controllers.Api
{
    [Route("api/posts")]
    [ApiController]
    public class PostApiController : BaseApiController
    {
        private readonly IPostService _postService;

        public PostApiController(
            IPostService postService)
        {
            _postService = postService;
        }

        // POST: /api/posts/create

        [Authorize(
            AuthenticationSchemes =
                JwtBearerDefaults.AuthenticationScheme)]
        [Currentu_id]
        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] CreatePostDTO model)
        {
            var u_id =
                (int)HttpContext.Items["u_id"]!;

            _postService.Create(
                model,
                u_id);

            return SendResponse<object?>(
                null,
                "Post created successfully.",
                HttpStatusCode.Created);
        }

        // PUT: /api/posts/update

        [Authorize(
            AuthenticationSchemes =
                JwtBearerDefaults.AuthenticationScheme)]
        [Currentu_id]
        [HttpPut("update")]
        public IActionResult Update(
            [FromBody] UpdatePostDTO model)
        {
            var u_id =
                (int)HttpContext.Items["u_id"]!;

            _postService.Update(
                model,
                u_id);

            return SendResponse<object?>(
                null,
                "Post updated successfully.",
                HttpStatusCode.OK);
        }

        // DELETE: /api/posts/{p_id}

        [Authorize(
            AuthenticationSchemes =
                JwtBearerDefaults.AuthenticationScheme)]
        [Currentu_id]
        [HttpDelete("{p_id:int}")]
        public IActionResult Delete(
            int p_id)
        {
            var u_id =
                (int)HttpContext.Items["u_id"]!;

            _postService.Delete(
                p_id,
                u_id);

            return SendResponse<object?>(
                null,
                "Post deleted successfully.",
                HttpStatusCode.OK);
        }

        // GET: /api/posts

        [HttpGet]
        public IActionResult GetPublicPosts()
        {
            var posts =
                _postService.GetPublicPosts();

            return SendResponse(
                posts,
                "Posts retrieved successfully.",
                HttpStatusCode.OK);
        }

        [Authorize(
            AuthenticationSchemes =
                JwtBearerDefaults.AuthenticationScheme)]
        [Currentu_id]
        [HttpGet("my")]
        public IActionResult GetMyPosts()
        {
            var u_id =
                (int)HttpContext.Items["u_id"]!;

            var posts =
                _postService.GetMyPosts(u_id);

            return SendResponse(
                posts,
                "My posts retrieved successfully.",
                HttpStatusCode.OK);
        }

        // GET: /api/posts/{p_id}

        [Authorize(
            AuthenticationSchemes =
                JwtBearerDefaults.AuthenticationScheme)]
        [Currentu_id]
        [HttpGet("{p_id:int}")]
        public IActionResult GetPost(
            int p_id)
        {
            var u_id =
                (int)HttpContext.Items["u_id"]!;

            var post =
                _postService.GetPost(
                    p_id,
                    u_id);

            if (post == null)
            {
                return SendError(
                    "Post not found.",
                    null,
                    HttpStatusCode.NotFound);
            }

            return SendResponse(
                post,
                "Post retrieved successfully.",
                HttpStatusCode.OK);
        }
    }
}