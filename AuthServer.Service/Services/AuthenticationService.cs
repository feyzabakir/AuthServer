﻿using AuthServer.Core.Configurations;
using AuthServer.Core.Dtos;
using AuthServer.Core.Interfaces;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;

namespace AuthServer.Service.Services;

public class AuthenticationService: IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserApp> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

    public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
    {
        _clients = optionsClient.Value;

        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _userRefreshTokenService = userRefreshTokenService;
    }

    public async Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return ResponseDto<TokenDto>.Fail("Email or Password is wrong", 400, true);

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return ResponseDto<TokenDto>.Fail("Email or Password is wrong", 400, true);
        }
        var token = _tokenService.CreateToken(user);

        var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

        if (userRefreshToken == null)
        {
            await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommitAsync();

        return ResponseDto<TokenDto>.Success(token, 200);
    }

    public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.SecretKey == clientLoginDto.ClientSecret);

        if (client == null)
        {
            return ResponseDto<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
        }

        var token = _tokenService.CreateTokenByClient(client);

        return ResponseDto<ClientTokenDto>.Success(token, 200);
    }

    public async Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

        if (existRefreshToken == null)
        {
            return ResponseDto<TokenDto>.Fail("Refresh token not found", 404, true);
        }

        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

        if (user == null)
        {
            return ResponseDto<TokenDto>.Fail("User Id not found", 404, true);
        }

        var tokenDto = _tokenService.CreateToken(user);

        existRefreshToken.Code = tokenDto.RefreshToken;
        existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

        await _unitOfWork.CommitAsync();

        return ResponseDto<TokenDto>.Success(tokenDto, 200);
    }

    public async Task<ResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
        if (existRefreshToken == null)
        {
            return ResponseDto<NoDataDto>.Fail("Refresh token not found", 404, true);
        }

        _userRefreshTokenService.Remove(existRefreshToken);

        await _unitOfWork.CommitAsync();

        return ResponseDto<NoDataDto>.Success(200);
    }
}
