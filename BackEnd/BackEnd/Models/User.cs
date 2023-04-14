using BackEnd.ModelsDto;

namespace BackEnd.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        //public string Gender { get; set; }
        public int? PictureId { get; set; }
        public Picture? Picture { get; set; }
        //public string Img_location { get; set; }
        public Auth Auth { get; set; }

        public List<Post> Posts { get; set; }
        //public List<User> Friends { get; set; }

        public User(string firstName, string lastName, string birthDate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
        }
    }
}
