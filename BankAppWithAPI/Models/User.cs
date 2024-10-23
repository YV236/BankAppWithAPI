using Microsoft.AspNetCore.Identity;

namespace BankAppWithAPI.Models
{
    public class User : IdentityUser
    {
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfCreation { get; set; }
        public virtual Card? Card { get; set; }
        public virtual List<BankAccountCard>? AccountCards { get; set; } 

    }
}
