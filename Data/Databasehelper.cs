using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PracticeApi.Data
{
   public interface IDatahelper
    {
        public SqlConnection GetConnection();

        Task<DataTable> ExecuteReaderAsync(string sql , params SqlParameter[] param);
        Task<int> ExecuteNonQueryAsync(string sql , params SqlParameter[] param);
        Task<object> ExecuteScalerAsync(string sql , params SqlParameter[] param);
    }
    public class Databasehelper
    {
        private readonly IConfiguration Config;
        private readonly string Con;
        public Databasehelper(IConfiguration _config)
        {
            Config = _config;
            Con = Config.GetConnectionString("DBCS");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(Con);
        }

        //public async Task<DataTable> ExecuteReaderAsync(string sql, params SqlParameter[] param)
        //{
        //    var dt = new DataTable();
        //    using var con = GetConnection();
        //    using var cmd = new SqlCommand(sql,con);
        //    if (param?.Any() == true) cmd.Parameters.AddRange(param);
        //    cmd.CommandType = CommandType.Text;
        //    await con.OpenAsync();
        //    var reader = await cmd.ExecuteReaderAsync();
        //    dt.Load(reader);
        //    return dt;
        //}

        public async Task<DataTable> ExecuteReaderAsyncold(string sql, List<SqlParameter> param)
        {
            using var con = GetConnection();

            using (var cmd = new SqlCommand(sql,con))
            {
                if (param!=null)
                {
                    cmd.Parameters.AddRange(param.ToArray());
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

        public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] param)
        {
            using var con = GetConnection();

            using (var cmd = new SqlCommand(sql, con))
            {
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }

                await con.OpenAsync();

                return await cmd.ExecuteNonQueryAsync();
            }

        }

        public async Task<object> ExecuteScalerAsync(string sql, params SqlParameter[] param)
        {
            using var con = GetConnection();

            using (var cmd = new SqlCommand(sql, con))
            {
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }

                await con.OpenAsync();

                return await cmd.ExecuteScalarAsync();
            }

        }

        public async Task<DataTable> ExecuteReaderAsync(string sql, params SqlParameter[] paramss)// Select Whole Datatable 
        {
            using var con = GetConnection();
            using (var cmd = new SqlCommand(sql ,con))
            {
                if (paramss!=null)
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

        public async Task<int> ExecuteNonQueryAsync1(string sql, params SqlParameter[] paramss ) // Insert Update Delete
        {
            using var con = GetConnection();

            using (var cmd = new SqlCommand(sql,con))
            {
                if (paramss!=null)
                {
                    cmd.Parameters.AddRange(paramss);
                }
                await con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> ExecuteScalerAsync1(string sql , params SqlParameter[] paramss) // For Single Row
        {
            using var con = GetConnection();
            using (var cmd = new SqlCommand(sql ,con))
            {
                if (paramss!=null)
                {
                    cmd.Parameters.AddRange(paramss);
                }
                await con.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }
        }
    }
}
