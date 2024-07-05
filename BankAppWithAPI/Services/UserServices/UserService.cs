
namespace BankAppWithAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<ServiceResponse<GetUserDto>> GetUserInfo(ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            var userId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var getUser = await _context.Users.FindAsync(userId);

            if(getUser == null)
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
    }
}
