using MySql.Data.MySqlClient;
using System.Data;

namespace ADO.NET_dapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=mailinglistdb;User ID=root;Password=yourpassword;"; 

            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    Console.WriteLine("Successfully connected to the database.");
                    db.Close();
                    Console.WriteLine("Successfully disconnected from the database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to the database: " + ex.Message);
                }
            }
        }
    }
}
