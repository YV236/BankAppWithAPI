namespace BankAppWithAPI.Models
{
    public class BankAccountCard
    {
        public int Id { get; set; }
        public BankAccount? Account { get; set; }
        public int AccountId { get; set; }
        public Card? Card { get; set; } 
        public int CardId { get; set; }
        public User? User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
