namespace BankAppWithAPI.Dtos.User
{
    public record UpdateUserDto(
        string UserFirstName,
        string UserLastName,
        string Street,
        string HomeNumber,
        string City,
        string Country,
        string PhoneNumber);
}
