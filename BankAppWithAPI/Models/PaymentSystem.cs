using System.Text.Json.Serialization;

namespace BankAppWithAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentSystem : byte
    {
        Visa = 4,
        MasterCard = 5
    }
}
