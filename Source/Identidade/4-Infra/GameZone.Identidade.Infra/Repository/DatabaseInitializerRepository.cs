using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Identidade.Infra.Repository
{
    public static class DatabaseInitializerRepository
    {
        public static void Initialize(string connectionString)
        {
            string outputDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string scriptsPath = Path.Combine(outputDirectory.Replace("/app/", ""), "Configurations", "InitialSetup-azure.sql");

            if (File.Exists(scriptsPath))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string scriptContent = File.ReadAllText(scriptsPath);

                    using (SqlCommand command = new SqlCommand(scriptContent, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
