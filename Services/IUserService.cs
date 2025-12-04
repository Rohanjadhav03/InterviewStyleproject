using PracticeApi.Models;

namespace PracticeApi.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(User user);
        Task<int> DeleteUserAsync(int id);
    }
}
