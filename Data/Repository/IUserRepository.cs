using PracticeApi.Models;

namespace PracticeApi.Data.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int Id);

        Task<int> CrateUserAsync(User user);

        Task<int> DeleteUserAsync(int Id);
    }
}
