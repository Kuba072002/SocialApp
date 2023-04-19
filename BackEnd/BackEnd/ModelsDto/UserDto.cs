using BackEnd.Models;

namespace BackEnd.ModelsDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Location { get; set; }
        public string Occupation { get; set; }
        public PictureDto? Picture { get; set; }
        public List<PostDto>? Posts { get; set; }
        public List<FriendDto>? Friends { get; set; }
    
        public UserDto(int id,string firstName,string lastName
            ,string birthDate,string location ,string occupation) 
        { 
            Id= id;
            FirstName= firstName;
            LastName= lastName;
            BirthDate= birthDate;
            Location= location;
            Occupation= occupation;
        }
    }
}
