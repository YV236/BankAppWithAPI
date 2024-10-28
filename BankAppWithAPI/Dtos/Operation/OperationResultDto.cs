using BankAppWithAPI.Models.Operations;

namespace BankAppWithAPI.Dtos.Operation
{
    public class OperationResultDto
    {
        public string IBAN { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public decimal BalanceAfter { get; set; } = decimal.Zero;
        public DateTime? OperationDate { get; set; }
        public OperationType? OperationType { get; set; }
    }
}
