namespace BankAppWithAPI.Dtos.User
{
    public record UserRegisterDto(
        string UserName,
        string UserSurname,
        string Email,
        string Address,
        string PhoneNumber);
    
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
}