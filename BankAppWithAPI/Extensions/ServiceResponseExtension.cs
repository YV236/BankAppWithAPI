using BankAppWithAPI.Models;
using System.Net;

namespace BankAppWithAPI.Extensions
{
    public static class ServiceResponseExtension
    {
        public static ServiceResponse<T> CreateErrorResponse<T>(this ServiceResponse<T> serviceResponse, T data, string message, HttpStatusCode statusCode)
        {
            serviceResponse.Data = data;
            serviceResponse.IsSuccessful = false;
            serviceResponse.Message = message;
            serviceResponse.StatusCode = statusCode;
            return serviceResponse;
        }
    }
}
