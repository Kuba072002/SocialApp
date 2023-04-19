namespace BackEnd.ModelsDto
{
    public class CommentDto
    {
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public PictureDto? UserPicture { get; set; }

        public CommentDto(string content, string createDate
            ,int userId,string userFirstName,string userLastName) { 
            Content= content; CreateDate= createDate; UserId= userId;
            UserFirstName= userFirstName; UserLastName= userLastName;
        }
    }
}
