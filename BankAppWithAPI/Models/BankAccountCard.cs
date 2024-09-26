namespace BankAppWithAPI.Models
{
    public class BankAccountCard
    {
        public Guid Id { get; set; }
        public BankAccount? Account { get; set; }
        public Guid AccountId { get; set; }
        public Card? Card { get; set; } 
        public Guid CardId { get; set; }
        public User? User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
