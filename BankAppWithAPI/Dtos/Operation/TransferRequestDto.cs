using BankAppWithAPI.Models.Operations;

namespace BankAppWithAPI.Dtos.Operation
{
    public class TransferRequestDto
    {
        public string DestinationIBAN { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
    }
}
