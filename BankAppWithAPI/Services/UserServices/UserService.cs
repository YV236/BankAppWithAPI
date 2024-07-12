using AutoMapper;
using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.User;
using BankAppWithAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BankAppWithAPI.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly UserManager<Models.User> _userManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(UserManager<Models.User> userManager, IMapper mapper, DataContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }


        public async Task<ServiceResponse<GetUserDto>> GetUserInfo(ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            var getUser = await FindUser(user);

            if (getUser == null)
            {
                serviceResponse.Data = null;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "The user is not found";
                return serviceResponse;
            }

            var userDto = _mapper.Map<GetUserDto>(getUser);
            serviceResponse.Data = userDto;
            serviceResponse.IsSuccessful = true;
            return serviceResponse;
        }
        
        private async Task<User> FindUser(ClaimsPrincipal userToFind)
        {
            var userId = userToFind.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var getUser = await _context.Users.FindAsync(userId);

            return getUser;
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool AreAllFieldsFilled(UserRegisterDto user)
        {
            var properties = user.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(user);
                
                if (property.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(value as string))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDto userRegisterDto)
        {
            var serviceResponse = new ServiceResponse<int>();

            if (!AreAllFieldsFilled(userRegisterDto))
            {
                serviceResponse.Data = 0;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "Error while registering. Some of the properties maybe filled incorrect";
                return serviceResponse;
            }else if (!IsValidEmail(userRegisterDto.Email))
            {
                serviceResponse.Data = 0;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = $"Error while registering. Email '{userRegisterDto.Email}' must to contain '@' and '.'";
                return serviceResponse;
            }

            var user = _mapper.Map<User>(userRegisterDto);
            user.UserName = userRegisterDto.Email;
            user.Address = $"{userRegisterDto.Street} {userRegisterDto.HomeNumber} {userRegisterDto.City} {userRegisterDto.Country}";
            user.DateOfCreation = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                serviceResponse.Data = 0;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                return serviceResponse;
            }

            serviceResponse.Data = 1;
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "User registered successfully";

            return serviceResponse;
        }

    }
}
