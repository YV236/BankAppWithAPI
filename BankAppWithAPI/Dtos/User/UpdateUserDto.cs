namespace BankAppWithAPI.Dtos.User
{
    public class UpdateUserDto
    {
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HomeNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
