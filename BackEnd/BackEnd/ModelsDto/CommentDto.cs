namespace BackEnd.ModelsDto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int UserId { get; set; }

        public CommentDto(int id,string content, string createDate,int userId) {
            Id = id; Content= content; CreateDate= createDate; UserId= userId;
        }
    }
}
