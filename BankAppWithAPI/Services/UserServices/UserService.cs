using AutoMapper;
using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.User;
using BankAppWithAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using BankAppWithAPI.Extensions;

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
                    return serviceResponse.CreateErrorResponse(null!, "The user is not found", HttpStatusCode.NotFound);

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

        public async Task<ServiceResponse<int>> Register(UserRegisterDto userRegisterDto)
        {
            var serviceResponse = new ServiceResponse<int>();

            if (!AreAllFieldsFilled(userRegisterDto))
                return serviceResponse.CreateErrorResponse(0,
                    "Error while registering. Some of the properties may be filled incorrectly", HttpStatusCode.UnprocessableEntity);

            if (!IsValidEmail(userRegisterDto.Email))
                return serviceResponse.CreateErrorResponse(0, 
                    $"Error while registering. Email '{userRegisterDto.Email}' must contain '@' and '.'", HttpStatusCode.UnprocessableEntity);

            if (userRegisterDto.PhoneNumber.Any(c => !char.IsDigit(c)) || userRegisterDto.PhoneNumber.Length < 9)
                return serviceResponse.CreateErrorResponse(0,
                    $"Error while registering. Phone number '{userRegisterDto.PhoneNumber}' must contain numbers only. And contain at least 9 digits",
                    HttpStatusCode.UnprocessableEntity);

            var user = _mapper.Map<User>(userRegisterDto);
            user.UserName = userRegisterDto.Email;
            user.Address = $"{userRegisterDto.Street} {userRegisterDto.HomeNumber} {userRegisterDto.City} {userRegisterDto.Country}";
            user.DateOfCreation = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
                return serviceResponse.CreateErrorResponse(0, string.Join(", ", result.Errors.Select(e => e.Description)),
                    HttpStatusCode.BadRequest);
            

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
                return serviceResponse.CreateErrorResponse(null!, "Error while updating. Some of the properties maybe filled incorrect",
                    HttpStatusCode.UnprocessableEntity);

             if (!userUpdateDto.PhoneNumber.All(char.IsDigit))
                return serviceResponse.CreateErrorResponse(null!, $"Error while registering. Phone number '{userUpdateDto.PhoneNumber}' must to contain numbers only",
                    HttpStatusCode.UnprocessableEntity);

            
            _mapper.Map(userUpdateDto, getUser);
            getUser.Address = $"{userUpdateDto.Street} {userUpdateDto.HomeNumber} {userUpdateDto.City} {userUpdateDto.Country}";
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetUserDto>(getUser);
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "The data successfully updated";
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
    }
}
