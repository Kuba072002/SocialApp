using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Services.UserService
{
    public interface IUserService
    {
        public string getMyName();
        Task<ServiceResponse<PictureDto>> getMyPicture();
        Task<ServiceResponse<UserDto>> getMe();
        Task<ServiceResponse> addPost(AddPostDto request);
        Task<ServiceResponse<List<PostDto>>> getMyPosts();
        Task<ServiceResponse> addFriend(int friendId);
        Task<ServiceResponse<List<FriendDto>>> getFriends(int userId);
    }
}
