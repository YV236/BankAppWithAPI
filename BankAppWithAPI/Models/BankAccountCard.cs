namespace BankAppWithAPI.Models
{
    public class BankAccountCard
    {
        public Guid Id { get; set; }
        public virtual BankAccount? Account { get; set; }
        public Guid AccountId { get; set; }
        public virtual Card? Card { get; set; } 
        public Guid CardId { get; set; }
        public virtual User? User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
