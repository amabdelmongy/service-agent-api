using System.Data.SqlClient;
using System.IO;

namespace Data
{
    public class RunDBScriptsRepository
    {
        private readonly string _connectionString;

        public RunDBScriptsRepository(
            string connectionString
        )
        {
            _connectionString = connectionString;
        }
        public void Run(string scriptPath)
        {
            if (File.Exists(scriptPath))
            {
                var sqlCreateDB = @"
                IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'db')
                BEGIN
                    create database[db];
                END;";

                var sql = File.ReadAllText(scriptPath);
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(sqlCreateDB, connection);
                    command.ExecuteNonQuery();
                    command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
