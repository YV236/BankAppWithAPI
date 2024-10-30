using System.Text.Json.Serialization;

namespace BankAppWithAPI.Models.Operations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationType
    {
        Deposit,
        Withdraw,
        Transfer
    }
}
