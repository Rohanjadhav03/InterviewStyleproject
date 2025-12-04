using System.Data;
using System.Data.SqlClient;

namespace PracticeApi.Data
{
    public interface IDBHelper
    {
        public SqlConnection GetConnection();
        Task<DataTable> ExecuteReaderAsync(string sql, params SqlParameter[] paramss);
        Task<int> ExecuteNonQueryAsync(string sql, List<SqlParameter> paramss);
        Task<object> ExecuteScalerAsync(string sql, params SqlParameter[] paramss);
    }
    public class DBHelper : IDBHelper
    {
        private readonly IConfiguration Config;
        private readonly string Connection;
        public DBHelper(IConfiguration _Config)
        {
            Config = _Config;
            Connection = Config.GetConnectionString("DBCS");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(Connection);
        }

        public async Task<DataTable> ExecuteReaderAsync(string sql, params SqlParameter[] paramss)
        {
            using var con = GetConnection();
            using (var cmd = new SqlCommand(sql, con))
            {
                if (paramss != null)
                {
                    cmd.Parameters.AddRange(paramss);
                }

                await con.OpenAsync();

                DataTable dt = new DataTable();

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
                return dt;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, List<SqlParameter> paramss)
        {
            using var con = GetConnection();
            using (var cmd = new SqlCommand(sql, con))
            {
                if (paramss != null)
                {
                    cmd.Parameters.AddRange(paramss.ToArray());
                }

                await con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }

        }
        public async Task<int> ExecuteNonQuerySpAsync(string sql, params SqlParameter[] paramss) // For Stored Procedure
        {
            using var con = GetConnection();
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (paramss != null)
                {
                    cmd.Parameters.AddRange(paramss);
                }
                await con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }

        }

        public async Task<object> ExecuteScalerAsync(string sql, params SqlParameter[] paramss)
        {
            using var con = GetConnection();
            using (var cmd = new SqlCommand(sql, con))
            {
                if (paramss != null)
                {
                    cmd.Parameters.AddRange(paramss);
                }

                await con.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }

        }
    }
}
