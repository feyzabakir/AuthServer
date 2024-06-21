using AuthServer.Core.Configurations;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;

namespace AuthServer.Core.Interfaces;

public interface ITokenService
{
    TokenDto CreateToken(UserApp userApp);

    ClientTokenDto CreateTokenByClient(Client client);
}