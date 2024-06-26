﻿namespace BankAppWithAPI.Dtos.User
{
    public class UserRegisterDto
    {
        public string UserName { get; set; } 
        public string? UserSurname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
