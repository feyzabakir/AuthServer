using AuthServer.Core.Configurations;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Interfaces;

public interface ITokenService
{
    TokenDto CreateToken(UserApp userApp);

    ClientTokenDto CreateTokenByClient(Client client);
}