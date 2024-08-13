namespace BankAppWithAPI.Dtos.Card
{
    public class GetCardDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
