using Azure;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Services.PictureService;
using BackEnd.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserController(IUserService userService,IPostService postService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _postService = postService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("GetMyId"), Authorize]
        public ActionResult<string> GetMyId()
        {
            return Ok(_userService.getMyId());
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
            if (_contextAccessor.HttpContext != null) {
                var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(userId != null)
                {
                    var response = await _userService.getUser(int.Parse(userId));
                    if (!response.Success)
                    {
                        return BadRequest(response.Message);
                    }
                    else
                    {
                        var friends = await _userService.getUserFriends(int.Parse(userId));
                        var posts = await _postService.getFriendsPosts(int.Parse(userId));
                        response.Data.Friends = friends.Data;
                        response.Data.Posts = posts.Data;
                    }
                    return Ok(response.Data);
                }
            }
            return BadRequest();
        }

        [HttpGet("GetUserProfile/{userId}"), Authorize]
        public async Task<ActionResult<UserDto>> GetUserProfile(int userId)
        {
            var response = await _userService.getUser(userId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            else
            {
                var friends = await _userService.getUserFriends(userId);
                var posts = await _postService.getUserPosts(userId);
                response.Data.Friends = friends.Data;
                response.Data.Posts = posts.Data;
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
