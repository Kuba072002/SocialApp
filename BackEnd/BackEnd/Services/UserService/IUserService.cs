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
        Task<ServiceResponse<UserDto>> getHomeData();
        Task<ServiceResponse<UserDto>> getUser(int userId);
        Task<ServiceResponse> addFriend(int friendId);
        Task<ServiceResponse<List<FriendDto>>> getUserFriends(int userId);
    }
}
