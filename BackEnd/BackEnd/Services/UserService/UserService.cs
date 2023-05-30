using Azure;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Services.PictureService;
using BackEnd.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Security.Claims;

namespace BackEnd.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPostService _postService;
        public UserService(DataContext context,IHttpContextAccessor contextAccessor,IPostService postService)
        {
            _context= context;
            _contextAccessor = contextAccessor;
            _postService = postService;
        }
        public string getMyId()
        {
            var result = string.Empty;
            if (_contextAccessor.HttpContext != null)
            {
                result = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return result;
        }

        public async Task<ServiceResponse<PictureDto>> getMyPicture()
        {
            
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Picture).FirstOrDefaultAsync(u => u.Id == userId);
                var picture = user.Picture;
                if (picture == null) {
                    return new ServiceResponse<PictureDto> { Success = false, Message = "" };
                }
                var response = new PictureDto(picture.Id,picture.Name,picture.Data,picture.FileExtension);
                return new ServiceResponse<PictureDto> { Data = response, Success = true, Message = "" };
            }
            return new ServiceResponse<PictureDto>{ Success = false, Message = "" };
        }

        public async Task<ServiceResponse<UserDto>> getHomeData()
        {
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Picture).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null) {
                    var response = new UserDto(user.Id,user.FirstName,user.LastName
                        ,user.BirthDate,user.Location,user.Occupation);
                    if (user.Picture != null)
                    {
                        response.Picture = new PictureDto(user.Picture.Id,user.Picture.Name
                            ,user.Picture.Data,user.Picture.FileExtension
                        );
                    }
                    var userPosts = await _postService.getFriendsPosts(userId);
                    if (userPosts.Success)
                        response.Posts = userPosts.Data;
                    var userFriends = await getUserFriends(userId);
                    if (userFriends.Success)
                        response.Friends = userFriends.Data;

                    return new ServiceResponse<UserDto> { Data = response, Success = true, Message = "Success" }; 
                }
            }
            return new ServiceResponse<UserDto> { Success = false, Message = "Fail" };

        }

        public async Task<ServiceResponse<UserDto>> getUser(int userId) {
            var user = await _context.Users.Include(u => u.Picture).FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var response = new UserDto(user.Id, user.FirstName, user.LastName
                        , user.BirthDate, user.Location, user.Occupation);
                if (user.Picture != null)
                {
                    response.Picture = new PictureDto(user.Picture.Id, user.Picture.Name
                        , user.Picture.Data, user.Picture.FileExtension
                    );
                }
                //var userPosts = await _postService.getUserPosts(userId);
                //if (userPosts.Success)
                //    response.Posts = userPosts.Data;
                //var userFriends = await getUserFriends(userId);
                //if (userFriends.Success)
                //    response.Friends = userFriends.Data;
                return new ServiceResponse<UserDto> { Data = response, Success = true, Message = "Success" };
            }
            return new ServiceResponse<UserDto> { Success = false, Message = "Fail" };
        }

        public async Task<ServiceResponse> addFriend(int friendId)
        {
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null && userId != friendId)
                {
                    var isFriendAlready = user.Friends.Any(f => f.FriendId == friendId);
                    if (!isFriendAlready) {
                        var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == friendId);
                        if (friend != null)
                        {
                            var userFriend = new UserFriend
                            {
                                UserId = userId,
                                User = user,
                                FriendId = friendId,
                                Friend = friend,
                                AddedDate = DateTime.Now.ToString("yyyy-MM-dd")
                            };

                            user.Friends.Add(userFriend);
                            _context.UserFriends.Add(userFriend);
                            await _context.SaveChangesAsync();
                            return new ServiceResponse { Success = true, Message = "Success" };
                        }
                    }
                    return new ServiceResponse { Success = false, Message = "Friend already" };
                }
            }
            return new ServiceResponse { Success = false, Message = "" };
        }
        public async Task<ServiceResponse<List<FriendDto>>> getUserFriends(int userId)
        {
            var friends = await _context.UserFriends
                .Include(uf => uf.Friend)
                .ThenInclude(u => u.Picture)
                .Where(uf => uf.UserId == userId)
                .Select(uf => new FriendDto
                {
                    Id = uf.FriendId,
                    FirstName = uf.Friend.FirstName,
                    LastName = uf.Friend.LastName,
                    AddedDate = uf.AddedDate,
                    Picture = uf.Friend.Picture == null ? null : new PictureDto
                        (uf.Friend.Picture.Id,uf.Friend.Picture.Name
                        ,uf.Friend.Picture.Data,uf.Friend.Picture.FileExtension)
                })
                //.OrderByDescending(f => f.AddedDate)
                .ToListAsync();
     
            return new ServiceResponse<List<FriendDto>> { Data = friends, Success = true, Message = "" };
        }

        private async Task<bool> UserExists(int userId)
        {
            if (await _context.Users.AnyAsync(u => u.Id == userId))
            {
                return true;
            }
            return false;
        }

    }
}
