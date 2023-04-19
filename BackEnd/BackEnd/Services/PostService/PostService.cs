using Azure;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Services.UserService;
using BackEnd.Utility;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace BackEnd.Services.PictureService
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public PostService(DataContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
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
                        CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Pictures = new List<Picture>(),
                        User = user,
                        UserId = userId
                    };
                    if (request.Pictures != null)
                    {
                        foreach (IFormFile p in request.Pictures)
                        {
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

        public async Task<ServiceResponse<List<PostDto>>> getFriendsPosts(int userId)
        {
            var friends = await _context.UserFriends
                .Include(uf => uf.Friend).ThenInclude(f => f.Posts).ThenInclude(p => p.Pictures)
                //.Include(uf => uf.Friend).ThenInclude(f => f.Posts).ThenInclude(p => p.Comments).ThenInclude(c => c.User).ThenInclude(u=> u.Picture)
                //.Include(uf => uf.Friend).ThenInclude(f => f.Posts).ThenInclude(p => p.Likes)
                .Include(uf => uf.Friend.Picture)
                .Where(uf => uf.UserId == userId).ToListAsync();
            List<PostDto> posts = new();
            foreach (var friend in friends)
            {
                foreach (var post in friend.Friend.Posts)
                {
                    var pictureDtos = new List<PictureDto>();
                    foreach (var picture in post.Pictures)
                    {
                        var pictureDto = new PictureDto(picture.Id,picture.Name
                            ,picture.Data,picture.FileExtension);
                        pictureDtos.Add(pictureDto);
                    }
                    var postDto = new PostDto(post.Id, post.Content, post.CreateDate,post.UserId);
                    postDto.Pictures= pictureDtos;
                    if(friend.Friend.Picture != null)
                    {
                        var pictureDto = new PictureDto(friend.Friend.Picture.Id,friend.Friend.Picture.Name
                            ,friend.Friend.Picture.Data, friend.Friend.Picture.FileExtension);
                        postDto.AddUser(friend.Friend.FirstName, friend.Friend.LastName, pictureDto);
                    }
                    else
                        postDto.AddUser(friend.Friend.FirstName, friend.Friend.LastName,null);
                    posts.Add(postDto);
                }
            }
            if (posts.Count > 0)
                posts = (List<PostDto>) posts.OrderByDescending(p => p.CreateDate);
            return new ServiceResponse<List<PostDto>> { Data = posts, Success = true, Message = "" };
        }

        public async Task<ServiceResponse<List<PostDto>>> getUserPosts(int userId)
        {
            var user = await _context.Users.Include(u => u.Posts).ThenInclude(p => p.Pictures).FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null && user.Posts != null)
            {
                List<PostDto> response = new();
                foreach (Post p in user.Posts.OrderByDescending(p => p.CreateDate))
                {
                    var post = new PostDto(p.Id, p.Content, p.CreateDate, p.UserId);
                    if (p.Pictures != null)
                    {
                        post.Pictures = new();
                        foreach (Picture pic in p.Pictures)
                        {
                            var picdto = new PictureDto(pic.Id,pic.Name,pic.Data,pic.FileExtension);
                            post.Pictures.Add(picdto);
                        }
                    }
                    response.Add(post);
                }
                return new ServiceResponse<List<PostDto>> { Data = response, Success = true, Message = "" };
            }
        
            return new ServiceResponse<List<PostDto>> { Success = false, Message = "" };
        }
    }
}
