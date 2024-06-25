namespace BankAppWithAPI.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountPriority { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public DateTime DateOfCreation { get; set; }

        public List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();
    }
}
