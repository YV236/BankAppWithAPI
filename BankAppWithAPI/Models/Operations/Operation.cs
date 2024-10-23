namespace BankAppWithAPI.Models.Operations
{
    public abstract class Operation
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; } = decimal.Zero;
        public decimal BalanceAfter { get; set; } = decimal.Zero;
        public DateTime OperationDate { get; set; }
        public OperationType OperationType { get; set; }

        public virtual BankAccount? Account { get; set; }
    }

}
