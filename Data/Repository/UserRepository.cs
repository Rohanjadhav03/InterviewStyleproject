using PracticeApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace PracticeApi.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDBHelper Db;

        public UserRepository(IDBHelper _Db)
        {
            Db = _Db;
        }

        public async Task<int> CrateUserAsync(User user)
        {
            string query = "INSERT INTO Usertbl (Username,UPassword) VALUES (@Username,@UPassword)";
            var param = new List<SqlParameter>
            {
                new SqlParameter("@Username",user.Username),
                new SqlParameter("@UPassword",user.Password),
            };

            return await Db.ExecuteNonQueryAsync(query,param);
        }

        public async Task<int> DeleteUserAsync(int Id)
        {
            string query = "DELETE FROM Usertbl WHERE UserId = @Id";
            var param = new List<SqlParameter>()
            {
                new SqlParameter("@Id", Id)
            };
            return await Db.ExecuteNonQueryAsync(query, param);
        }

        public async Task<User> GetUserByIdAsync(int Id)
        {
            string query = "SELECT UserId ,Username,UPassword FROM Usertbl WHERE UserId=@ID";
            var param = new SqlParameter("@ID", Id);
            DataTable dt = await Db.ExecuteReaderAsync(query,param);
            if (dt.Rows.Count==0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];

            return new User
            {
                UserID = Convert.ToInt32(row["UserId"]),
                Username = row["Username"].ToString(),
                Password = row["UPassword"].ToString()
            };
        }


    }
}
