using Echo.Business.Dtos;
using Echo.Business.DataProtection;
using Echo.Business.Operations.User.Dtos;
using Echo.Business.Shared;
using Echo.Data.Entities;
using Echo.Data.Repositories;
using Echo.Data.UnitOfWork;

namespace Echo.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataProtection _dataProtection;

        public UserManager(
            IRepository<UserEntity> userRepository,
            IUnitOfWork unitOfWork,
            IDataProtection dataProtection)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _dataProtection = dataProtection;
        }

        // Used for manually adding a user (e.g., by admin)
        public async Task<ServiceMessage<UserInfoDto>> AddUserAsync(AddUserDto dto)
        {
            try
            {
                var existingUser = await _userRepository.GetAsync(
                    x => x.Email.ToLower() == dto.Email.ToLower());

                if (existingUser != null)
                {
                    return new ServiceMessage<UserInfoDto>
                    {
                        IsSucceed = false,
                        Message = "This email is already registered."
                    };
                }

                var entity = new UserEntity
                {
                    Email = dto.Email,
                    PasswordHash = _dataProtection.Protect(dto.Password),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    UserRole = dto.UserRole,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Message = "User added successfully.",
                    Data = new UserInfoDto
                    {
                        Id = entity.Id,
                        Email = entity.Email,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        UserRole = entity.UserRole
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = $"An error occurred while adding the user: {ex.Message}"
                };
            }
        }

        // Verifies user credentials and returns user info
        public async Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetAsync(
                x => x.Email.ToLower() == dto.Email.ToLower());

            if (user == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Incorrect email or password."
                };
            }

            var decryptedPassword = _dataProtection.Unprotect(user.PasswordHash);

            if (decryptedPassword != dto.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Incorrect email or password."
                };
            }

            return new ServiceMessage<UserInfoDto>
            {
                IsSucceed = true,
                Message = "Login successful.",
                Data = new UserInfoDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserRole = user.UserRole
                }
            };
        }

        // Not used, can be removed or implemented later
        public object LoginUser(LoginUserDto dto)
        {
            throw new NotImplementedException();
        }

        // To be implemented: user registration with validations
        public Task<ServiceMessage<UserInfoDto>> RegisterUserAsync(AddUserDto dto)
        {
            throw new NotImplementedException();
        }
    }

    // Generic service response wrapper
    public class ServiceMessage<T>
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
    }
}