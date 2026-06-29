using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace DAL
{
    public class DALBackupRestore
    {
        private string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=hotel_;Integrated Security=True;TrustServerCertificate=True";
        private string conexionStringMaster = "Data Source=.\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";

        public void BackupDatabase(string backupFilePath)
        {  
            string query = "BACKUP DATABASE [hotel_] TO DISK = @ruta WITH NOFORMAT, NOINIT, NAME = N'Backup-Completo', SKIP, NOREWIND, NOUNLOAD, STATS = 10";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ruta", backupFilePath);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void RestoreDatabase(string backupFilePath)
        {
            using (SqlConnection connectionMaster = new SqlConnection(conexionStringMaster))
            {
                connectionMaster.Open();
                string sqlsingluser = "ALTER DATABASE [hotel_] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                using (SqlCommand command = new SqlCommand(sqlsingluser, connectionMaster))
                {
                    command.ExecuteNonQuery();
                }

                string sqlRestore = "RESTORE DATABASE [hotel_] FROM DISK = @ruta WITH REPLACE";
                using (SqlCommand commandRestore = new SqlCommand(sqlRestore, connectionMaster))
                {
                    commandRestore.Parameters.AddWithValue("@ruta", backupFilePath);
                    commandRestore.ExecuteNonQuery();
                }
                string sqlmultiuser = "ALTER DATABASE [hotel_] SET MULTI_USER";
                using (SqlCommand commandMultiUser = new SqlCommand(sqlmultiuser, connectionMaster))
                {
                    commandMultiUser.ExecuteNonQuery();
                }
            }
        }
    }
}
