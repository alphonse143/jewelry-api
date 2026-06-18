using JewelryAPI.Application.DTOs;

namespace JewelryAPI.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
