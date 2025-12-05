using Microsoft.AspNetCore.Mvc.Formatters;
using PracticeApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace PracticeApi.Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDBHelper DB;
        public AuthRepository(IDBHelper _db)
        {
            DB = _db;
        }

        public async Task<UserAuth> GetUserByUserNameAsync(string username)
        {
            string query = "SELECT * FROM Usertbl WHERE Username = @Username";
            var param = new SqlParameter("@Username", username);
            DataTable dt = await DB.ExecuteReaderAsync(query, param);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];

            return new UserAuth
            {
                UserId = Convert.ToInt32(row["UserId"]),
                Username = row["Username"].ToString(),
                Password = row["UPassword"].ToString(),
                Role = row["Role"].ToString()
            };
        }

        public Task<int> RegisterAsync(UserAuth user)
        {
            string query = "INSERT INTO Usertbl (Username,UPassword,Role) VALUES(@Username,@UPassword,@Role)";

            var param = new List<SqlParameter>
            {
                new SqlParameter("@Username",user.Username),
                new SqlParameter("@UPassword",user.Password),
                new SqlParameter("@Role", user.Role)
            };
            return DB.ExecuteNonQueryAsync(query, param);
        }

        public async Task<int> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry)
        {
            string query = "UPDATE Usertbl SET RefreshToken=@RefreshToken, RefreshTokenExpiry=@Expiry WHERE UserId=@UserId";

            var param = new List<SqlParameter>
            {
               new SqlParameter("@RefreshToken", refreshToken),
               new SqlParameter("@Expiry", expiry),
               new SqlParameter("@UserId", userId)
            };

            return await DB.ExecuteNonQueryAsync(query, param);
        }

        public async Task<UserAuth> GetUserByRefreshTokenAsync(string refreshToken)
        {
            string query = "SELECT * FROM Usertbl WHERE RefreshToken = @refreshToken";

            var param = new SqlParameter("@refreshToken", refreshToken);

            DataTable dt = await DB.ExecuteReaderAsync(query, param);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new UserAuth
            {
                UserId = Convert.ToInt32(row["UserId"]),
                Username = row["Username"].ToString(),
                Password = row["UPassword"].ToString(),
                RefreshToken = row["RefreshToken"].ToString(),
                RefreshTokenExpiry = Convert.ToDateTime(row["RefreshTokenExpiry"]),
                Role = row["Role"].ToString()
            };
        }
    }
}
