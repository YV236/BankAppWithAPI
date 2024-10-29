using BankAppWithAPI.Models.Operations;

namespace BankAppWithAPI.Dtos.Operation
{
    public class OperationRequestDto
    {
        public string IBAN { get; set; } = string.Empty;
        public string DestinationIBAN { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        public OperationType OperationType { get; set; }
    }
}
