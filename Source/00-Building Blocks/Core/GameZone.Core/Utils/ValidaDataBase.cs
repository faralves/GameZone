using Microsoft.Data.SqlClient;

namespace GameZone.Core.Utils
{
    public static class ValidaDataBase
    {
        private static int tentativasConexao = 0;
        public static bool CheckIfDatabaseExists(string connectionString, string databaseName)
        {
            try
            {
                tentativasConexao ++;

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
            catch (Exception)
            {
                Task.Delay(10000).GetAwaiter().GetResult(); // Aguarde 10 segundos antes de tentar a conexão

                if (tentativasConexao <= 20)
                    CheckIfDatabaseExists(connectionString, databaseName);

                return false;
            }
        }
    }
}
