namespace BankAppWithAPI.Models
{
    public class BankAccountCard
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public BankAccount? Account { get; set; }
        public int CardId { get; set; }
        public Card? Card { get; set; } 
    }
}
