namespace BackEnd.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Img_location { get; set; }
        public int AuthId { get; set; }
        public Auth Auth { get; set; }
    }
}
