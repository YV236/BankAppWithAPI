using Microsoft.AspNetCore.Identity;

namespace BankAppWithAPI.Models
{
    public class User : IdentityUser
    {
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfCreation { get; set; }
        public List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();

    }
}
