using PracticeApi.Data.Repository;
using PracticeApi.Models;

namespace PracticeApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository Repo;
        public UserService(IUserRepository _Repo)
        {
            Repo = _Repo;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new Exception("Username Is Required..");
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new Exception("Password Is Required..");
            }

            return await Repo.CrateUserAsync(user);
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            if (id < 0)
            {
                throw new Exception("InValid User Id..");
            }
            return await Repo.DeleteUserAsync(id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id<0)
            {
                throw new Exception("InValid User Id..");
            }
            var user = await Repo.GetUserByIdAsync(id);
            if (user==null)
            {
                throw new Exception("User Not Found..");

            }
            return user;
        }
    }
}
