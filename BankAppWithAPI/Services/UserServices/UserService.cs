using AutoMapper;
using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.User;
using BankAppWithAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BankAppWithAPI.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(UserManager<User> userManager, IMapper mapper, DataContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserInfo(ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                var getUser = await FindUser(user);

                if (getUser == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Message = "The user is not found";
                    serviceResponse.StatusCode = HttpStatusCode.NotFound;
                    return serviceResponse;
                }

                var userDto = _mapper.Map<GetUserDto>(getUser);
                serviceResponse.Data = userDto;
                serviceResponse.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return serviceResponse;
        }
        
        private async Task<User> FindUser(ClaimsPrincipal userToFind)
        {
            var userId = userToFind.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var getUser = await _context.Users.Include(u => u.Card).FirstOrDefaultAsync(u => u.Id == userId);

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

        private bool AreAllFieldsFilled(UpdateUserDto user)
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
                serviceResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
                return serviceResponse;
            }else if (!IsValidEmail(userRegisterDto.Email))
            {
                serviceResponse.Data = 0;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = $"Error while registering. Email '{userRegisterDto.Email}' must to contain '@' and '.'";
                serviceResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
                return serviceResponse;
            }else if(!userRegisterDto.PhoneNumber.All(char.IsDigit))
            {
                serviceResponse.Data = 0;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = $"Error while registering. Phone number '{userRegisterDto.PhoneNumber}' must to contain numbers only";
                serviceResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
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
                serviceResponse.StatusCode = HttpStatusCode.BadRequest;
                return serviceResponse;
            }

            serviceResponse.Data = 1;
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "User registered successfully";

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUserInfo(ClaimsPrincipal user, UpdateUserDto userUpdateDto)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();
            var getUser = await FindUser(user);

            if (!AreAllFieldsFilled(userUpdateDto))
            {
                serviceResponse.Data = null;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "Error while updating. Some of the properties maybe filled incorrect";
                serviceResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
                return serviceResponse;
            }
            else if (!userUpdateDto.PhoneNumber.All(char.IsDigit))
            {
                serviceResponse.Data = null;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = $"Error while registering. Phone number '{userUpdateDto.PhoneNumber}' must to contain numbers only";
                serviceResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
                return serviceResponse;
            }

            _mapper.Map(userUpdateDto, getUser);
            getUser.Address = $"{userUpdateDto.Street} {userUpdateDto.HomeNumber} {userUpdateDto.City} {userUpdateDto.Country}";
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetUserDto>(getUser);
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "The data successfully updated";
            return serviceResponse;
        }
    }
}
