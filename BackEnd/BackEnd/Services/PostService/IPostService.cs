using BackEnd.ModelsDto;
using BackEnd.Utility;

namespace BackEnd.Services.PictureService
{
    public interface IPostService
    {
        Task<ServiceResponse> addPost(AddPostDto request);
        Task<ServiceResponse<List<PostDto>>> getUserPosts(int userId);
        Task<ServiceResponse<List<PostDto>>> getFriendsPosts(int userId);
    }
}
