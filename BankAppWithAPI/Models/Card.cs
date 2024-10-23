namespace BankAppWithAPI.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public PaymentSystem? PaymentSystem { get; set; } 
        public byte[] PinHash { get; set; } = new byte[0];
        public byte[] PinSalt { get; set; } = new byte[0];
        public byte[] CVVHash { get; set; } = new byte[0];
        public byte[] CVVSalt { get; set; } = new byte[0];
        public DateTime ExpiryDate { get; set; }
        public virtual User? User { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();
    }
}
