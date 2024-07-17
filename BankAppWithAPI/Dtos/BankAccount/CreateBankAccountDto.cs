namespace BankAppWithAPI.Dtos.BankAccount
{
    public class CreateBankAccountDto
    {
        public string AccountPriority { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
    }
}
