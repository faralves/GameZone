using Microsoft.Data.SqlClient;

namespace GameZone.Core.Utils
{
    public static class ValidaDataBase
    {
        public static bool CheckIfDatabaseExists(string connectionString, string databaseName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int databaseCount = (int)command.ExecuteScalar();

                    return databaseCount > 0;
                }
            }
        }
    }
}
