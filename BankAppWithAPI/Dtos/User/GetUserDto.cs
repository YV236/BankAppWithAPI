﻿namespace BankAppWithAPI.Dtos.User
{
    public class GetUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
