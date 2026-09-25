using Microsoft.AspNetCore.Mvc;
using PosWebApplication.DTOs.Post;
using PosWebApplication.Services.Post;
using System.Net;
using PosWebApplication.DTOs.Post;
using PosWebApplication.Services.Post;

namespace PosWebApplication.Controllers.Api
{
    [Route("api/posts")]
    [ApiController]
    public class PostApiController : BaseApiController
    {
        private readonly IPostService _postService;

        public PostApiController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] CreatePostDTO model,
            [FromQuery] int u_id)
        {
            _postService.Create(model, u_id);

            return SendResponse<object?>(
                null,
                "Post created successfully.",
                HttpStatusCode.Created);
        }

        [HttpGet]
        public IActionResult GetPublicPosts()
        {
            var posts = _postService.GetPublicPosts();

            return SendResponse(
                posts,
                "Posts retrieved successfully.",
                HttpStatusCode.OK);
        }

        [HttpGet("my")]
        public IActionResult GetMyPosts(
            [FromQuery] int u_id)
        {
            var posts = _postService.GetMyPosts(u_id);

            return SendResponse(
                posts,
                "My posts retrieved successfully.",
                HttpStatusCode.OK);
        }

        [HttpGet("{p_id}")]
        public IActionResult GetPost(
            int p_id,
            [FromQuery] int u_id)
        {
            var post = _postService.GetPost(
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

        [HttpPut("update")]
        public IActionResult Update(
            [FromBody] UpdatePostDTO model,
            [FromQuery] int u_id)
        {
            _postService.Update(model, u_id);

            return SendResponse<object?>(
                null,
                "Post updated successfully.",
                HttpStatusCode.OK);
        }

        [HttpDelete("{p_id}")]
        public IActionResult Delete(
            int p_id,
            [FromQuery] int u_id)
        {
            _postService.Delete(p_id, u_id);

            return SendResponse<object?>(
                null,
                "Post deleted successfully.",
                HttpStatusCode.OK);
        }
    }
}