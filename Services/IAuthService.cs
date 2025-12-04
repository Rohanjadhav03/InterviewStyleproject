using Microsoft.AspNetCore.Mvc;
using PracticeApi.Models;
using PracticeApi.Models.Dtos;

namespace PracticeApi.Services
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(RegisterRequeset requeset);
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    }
}
