﻿namespace BankAppWithAPI.Dtos.BankAccount
{
    public class GetBankAccountDto
    {
        public string IBAN { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string AccountPriority { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
    }
}
