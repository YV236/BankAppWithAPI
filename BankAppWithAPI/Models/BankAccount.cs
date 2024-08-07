﻿namespace BankAppWithAPI.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string AccountPriority { get; set; } = string.Empty;
        public string AccountName { get; set; } = "MyBankAccount";
        public DateTime DateOfCreation { get; set; }

        public List<BankAccountCard> AccountCards { get; set; } = new List<BankAccountCard>();
    }
}
