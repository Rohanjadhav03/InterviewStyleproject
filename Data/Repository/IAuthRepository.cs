using PracticeApi.Models;

namespace PracticeApi.Data.Repository
{
    public interface IAuthRepository
    {
        Task<int> RegisterAsync(UserAuth user);
        Task<UserAuth> GetUserByUserNameAsync(string username);
        Task<int> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
        Task<UserAuth> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
