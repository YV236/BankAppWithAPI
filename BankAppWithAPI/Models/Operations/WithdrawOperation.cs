namespace BankAppWithAPI.Models.Operations
{
    public class WithdrawOperation : Operation
    {
        public override OperationType OperationType => OperationType.Withdraw;
    }
}
