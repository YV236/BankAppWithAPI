namespace BankAppWithAPI.Dtos.User
{
    public record UserRegisterDto(
        string UserFirstName,
        string UserLastName,
        string Email,
        string Street,
        string HomeNumber,
        string City,
        string Country,
        string Password,
        string PhoneNumber);
    
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
}