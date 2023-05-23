using BackEnd.ModelsDto;
using BackEnd.Services.PictureService;
using BackEnd.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("AddPost"), Authorize]
        public async Task<ActionResult<string>> AddPost([FromForm] AddPostDto request)
        {
            var response = await _postService.addPost(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpGet("GetUserPosts/{userId}"), Authorize]
        public async Task<ActionResult<List<PostDto>>> GetUserPosts(int userId)
        {
            var response = await _postService.getUserPosts(userId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
    }
}
