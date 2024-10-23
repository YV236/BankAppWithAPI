namespace BankAppWithAPI.Models.Operations
{
    public class TransferOperation : Operation
    {
        public Guid DestinationAccountId { get; set; }
        public virtual BankAccount? DestinationAccount { get; set; }
    }
}
