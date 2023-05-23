using BackEnd.ModelsDto;

namespace BackEnd.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public int? PictureId { get; set; }
        public Picture? Picture { get; set; }
        public Auth Auth { get; set; }
        public List<Post> Posts { get; set; }
        public List<UserFriend> Friends { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }


        public User(string firstName, string lastName, string birthDate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
        }
    }
}
