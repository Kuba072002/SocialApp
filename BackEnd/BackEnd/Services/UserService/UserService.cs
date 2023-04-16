using Azure;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.ModelsDto;
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
        public UserService(DataContext context,IHttpContextAccessor contextAccessor)
        {
            _context= context;
            _contextAccessor = contextAccessor;
        }
        public string getMyName()
        {
            var result = string.Empty;
            if (_contextAccessor.HttpContext != null)
            {
                result = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
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
                var response = new PictureDto { 
                    Id= picture.Id,
                    Name= picture.Name,
                    Data= picture.Data,
                    FileExtension= picture.FileExtension
                };
                return new ServiceResponse<PictureDto> { Data = response, Success = true, Message = "" };
            }
            return new ServiceResponse<PictureDto>{ Success = false, Message = "" };
        }

        public async Task<ServiceResponse<UserDto>> getMe()
        {
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Picture).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null) {
                    var response = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                    };
                    if (user.Picture != null)
                    {
                        response.Picture = new PictureDto
                        {
                            Id = user.Picture.Id,
                            Name = user.Picture.Name,
                            Data = user.Picture.Data,
                            FileExtension = user.Picture.FileExtension
                        };
                    }
                    return new ServiceResponse<UserDto> { Data = response, Success = true, Message = "Success" }; 
                }
            }
            return new ServiceResponse<UserDto> { Success = false, Message = "Fail" };

        }

        public async Task<ServiceResponse> addPost(AddPostDto request)
        {
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Posts).FirstOrDefaultAsync(u => u.Id == userId);
                string s = "Post added successful!";
                if (user != null)
                {
                    var post = new Post
                    {
                        Content = request.Content,
                        Pictures = new List<Picture>(),
                        User= user,
                        UserId= userId
                    };
                    if (request.Pictures != null)
                    {
                        foreach (IFormFile p in request.Pictures) {
                            if (p.Length > 0)
                            {
                                var picture = new Picture();
                                picture.Create(p);
                                _context.Pictures.Add(picture);
                                post.Pictures.Add(picture);
                            } 
                        }
                        s = "With pictures";
                    }
                    _context.Posts.Add(post);
                    user.Posts.Add(post);
                    await _context.SaveChangesAsync();
                    return new ServiceResponse { Success = true, Message = s };
                } 
            }
            return new ServiceResponse { Success = false, Message = "" };
        }

        public async Task<ServiceResponse<List<PostDto>>> getMyPosts()
        {
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Posts).ThenInclude(p => p.Pictures).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null && user.Posts != null)
                {
                    List<PostDto> response = new();
                    foreach (Post p in user.Posts)
                    {
                        var post = new PostDto { 
                            Id = p.Id, 
                            Content = p.Content, 
                            CreateDate = p.CreateDate , 
                            UserId = p.UserId,
                            Pictures = new() 
                        };
                        if(p.Pictures != null)
                        {
                            foreach(Picture pic in p.Pictures)
                            {
                                var picdto = new PictureDto
                                {
                                    Id = pic.Id,
                                    Name = pic.Name,
                                    Data = pic.Data,
                                    FileExtension = pic.FileExtension
                                };
                                post.Pictures.Add(picdto);
                            }
                        }
                        response.Add(post);
                    }
                    return new ServiceResponse<List<PostDto>> { Data=response, Success = true, Message = "" };
                }
            }
            return new ServiceResponse<List<PostDto>> { Success=false, Message = "" };
        }

        public async Task<ServiceResponse> addFriend(int friendId)
        {
            if (_contextAccessor.HttpContext != null)
            {
                int userId = Int32.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null && userId != friendId)
                {
                    var isFriendAlready = user.Friends.FirstOrDefault(f => f.FriendId == friendId);
                    if (isFriendAlready == null) {
                        var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == friendId);
                        if (friend != null)
                        {
                            var userFriend = new UserFriend
                            {
                                UserId = userId,
                                User = user,
                                FriendId = friendId,
                                Friend = friend
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
        public async Task<ServiceResponse<List<FriendDto>>> getFriends(int userId)
        {
            //var user = await _context.Users.Include(u => u.Friends).ThenInclude(p => p.Picture).FirstOrDefaultAsync(u => u.Id == userId);
            var friends = await _context.UserFriends
                .Include(uf => uf.Friend)
                .ThenInclude(u => u.Picture)
                .Where(uf => uf.UserId == userId)
                .Select(uf => new FriendDto
                {
                    Id = uf.FriendId,
                    FirstName = uf.Friend.FirstName,
                    LastName = uf.Friend.LastName,
                    Picture = uf.Friend.Picture == null ? null : new PictureDto
                    {
                        Id = uf.Friend.Picture.Id,
                        Name = uf.Friend.Picture.Name,
                        Data = uf.Friend.Picture.Data,
                        FileExtension = uf.Friend.Picture.FileExtension
                    }
                })
                .ToListAsync();
     
            return new ServiceResponse<List<FriendDto>> { Data = friends, Success = true, Message = "" };
        }
    }
}
