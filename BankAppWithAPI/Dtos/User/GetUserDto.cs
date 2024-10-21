using BankAppWithAPI.Dtos.Card;

namespace BankAppWithAPI.Dtos.User
{
    public class GetUserDto
    {
        public string? UserName { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public GetCardDto? Card { get; set; }
    }
}
