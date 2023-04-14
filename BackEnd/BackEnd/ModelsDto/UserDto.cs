using BackEnd.Models;

namespace BackEnd.ModelsDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public PictureDto? Picture { get; set; }
    }
}
