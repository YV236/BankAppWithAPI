namespace BankAppWithAPI.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string IBAN { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsActive { get; set; } = false;
        public string AccountName { get; set; } = "MyBankAccount";
        public DateTime DateOfCreation { get; set; }

        public List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();
    }
}
