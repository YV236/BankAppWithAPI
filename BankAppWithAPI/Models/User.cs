namespace BankAppWithAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        public DateTime DateOfCreation { get; set; }
        public List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();

    }
}
