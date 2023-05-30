using BackEnd.Models;

namespace BackEnd.ModelsDto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int UserId { get; set; }
        public List<PictureDto>? Pictures { get; set; }
        public List<int>? Likes { get; set; }
        public List<CommentDto>? Comments { get; set; }

        public PostDto(int id, string content, string createDate, int userId) {
            Id = id;
            Content = content;
            CreateDate = createDate;
            UserId = userId;
        }
    }
}
