namespace PracticeApi.Models
{
    public class UserAuth
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public string Role { get; set; }
    }
}
