namespace BankAppWithAPI.Dtos.Card
{
    public class LoginCardDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public short PinCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
