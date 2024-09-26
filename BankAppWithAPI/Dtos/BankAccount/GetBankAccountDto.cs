namespace BankAppWithAPI.Dtos.BankAccount
{
    public class GetBankAccountDto
    {
        public string IBAN { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public string AccountName { get; set; } = string.Empty;
    }
}
