
namespace BankAppWithAPI.Dtos.Operation
{
    public class OperationRequestDto
    {
        public Models.BankAccount? Account { get; set; }

        public int? Amount { get; set; }

    }
}
