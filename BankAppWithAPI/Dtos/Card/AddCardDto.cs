using BankAppWithAPI.Models;

namespace BankAppWithAPI.Dtos.Card
{
    public class AddCardDto
    {
        public PaymentSystem? PaymentSystem { get; set; }
        public string PinCode { get; set; } = string.Empty;
    }
}
