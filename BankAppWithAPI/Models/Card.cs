namespace BankAppWithAPI.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public byte[] CVVHash { get; set; } = new byte[0];
        public byte[] CVVSalt { get; set; } = new byte[0];
        public DateTime ExpiryDate { get; set; }
        public List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();
    }
}
