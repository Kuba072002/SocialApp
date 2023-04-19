using BackEnd.Models;

namespace BackEnd.ModelsDto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public PictureDto? UserPicture { get; set; }
        public List<PictureDto>? Pictures { get; set; }

        public PostDto(int id, string content, string createDate, int userId) {
            Id = id;
            Content = content;
            CreateDate = createDate;
            UserId = userId;
        }

        public void AddUser(string firstName,string lastName,PictureDto? picture)
        {
            FirstName= firstName;
            LastName= lastName;
            UserPicture = picture;
        }
    }
}
