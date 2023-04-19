using Azure;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Services.PictureService;
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
        private readonly IPostService _postService;
        public UserController(IUserService userService,IPostService postService)
        {
            _userService = userService;
            _postService = postService;
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

        [HttpGet("getHomeData"), Authorize]
        public async Task<ActionResult<UserDto>> getHomeData()
        {
            var response = await _userService.getHomeData();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("GetUser/{userId}"), Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int userId)
        {
            var response = await _userService.getUser(userId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
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

        [HttpPost("AddFriend"), Authorize]
        public async Task<ActionResult<string>> AddFriend(int friendId)
        {
            var response = await _userService.addFriend(friendId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpGet("GetUserFriends"), Authorize]
        public async Task<ActionResult<List<FriendDto>>> GetUserFriends(int userId)
        {
            var response = await _userService.getUserFriends(userId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
    }
}
