using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Interfaces;

public interface IAuthenticationService
{
    Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto, CancellationToken cancellationToken);

    Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken);

    Task<ResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken, CancellationToken cancellationToken);

    Task<ResponseDto<ClientTokenDto>> CreateTokenByClientAsync(ClientLoginDto clientLoginDto, CancellationToken cancellationToken);
}
