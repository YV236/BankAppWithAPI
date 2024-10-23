using BankAppWithAPI.Models.Operations;

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

        public virtual List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();
        public virtual List<Operation> Operations { get; set; } = new List<Operation>();
    }
}
