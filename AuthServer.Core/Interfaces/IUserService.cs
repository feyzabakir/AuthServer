using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Interfaces;

public interface IUserService
{
    Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);

    Task<ResponseDto<UserAppDto>> GetUserByNameAsync(string userName, CancellationToken cancellationToken);
}
