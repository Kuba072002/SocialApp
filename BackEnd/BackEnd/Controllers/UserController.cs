using Azure;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("GetMyName"), Authorize]
        public ActionResult<string> GetMyName()
        {
            return Ok(_userService.getMyName());
        }

        [HttpGet("GetMyPicture"), Authorize]
        public async Task<ActionResult<PictureDto>> GetMyPicture()
        {
            var response = await _userService.getMyPicture();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("GetMe"), Authorize]
        public async Task<ActionResult<PictureDto>> GetMe()
        {
            var response = await _userService.getMe();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("AddPost"), Authorize]
        public async Task<ActionResult<string>> AddPost([FromForm] AddPostDto request)
        {
            var response = await _userService.addPost(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpGet("GetMyPosts"), Authorize]
        public async Task<ActionResult<List<PostDto>>> GetMyPosts()
        {
            var response = await _userService.getMyPosts();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
    }
}
