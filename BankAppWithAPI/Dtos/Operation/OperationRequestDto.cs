using BankAppWithAPI.Models.Operations;

namespace BankAppWithAPI.Dtos.Operation
{
    public class OperationRequestDto
    {
        public Models.BankAccount? Account { get; set; }
        public Models.BankAccount? DestinationAccount { get; set; }
        public decimal? Amount { get; set; }
        public OperationType OperationType { get; set; }
    }
}
