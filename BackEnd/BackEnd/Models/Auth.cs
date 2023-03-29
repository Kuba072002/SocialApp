namespace BackEnd.Models
{
    public class Auth
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
        //nowe
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
