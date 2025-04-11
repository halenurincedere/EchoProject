using Echo.Business.Dtos;
using Echo.Business.Operations.User.Dtos;

namespace Echo.Business.Operations.User
{
    public interface IUserService
    {
        // Registers a new user and returns user info if successful
        Task<ServiceMessage<UserInfoDto>> RegisterUserAsync(AddUserDto dto);

        // Adds a user directly (used for admin operations maybe)
        Task<ServiceMessage<UserInfoDto>> AddUserAsync(AddUserDto userDto);

        // Authenticates user and returns login result (like token or user info)
        Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto dto);

        // (Optional) Legacy or temporary usage — not strongly typed
        object LoginUser(LoginUserDto dto); 
    }
}