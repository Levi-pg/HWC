using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
namespace realEstate_DataBase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serverKapcsolat = new MySqlConnectionStringBuilder { Server = "127.0.0.1", Database = "ingatlan_", UserID = "root", Password = "" };
            MySqlConnection kapcsolat;
            kapcsolat = new MySqlConnection(serverKapcsolat.ConnectionString);
            kapcsolat.Open();
            var lekerdezes = kapcsolat.CreateCommand();
            Console.Write("Kérem az emeltetet: ");
            string emelet = Console.ReadLine();
            lekerdezes.CommandText = $"Select avg(area) from realestates where floors = {emelet}";
            var olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"A(z) {emelet}. emeltei ingatlanok átlagterülete {olvaso.GetDouble(0):0.00} m2");
            }
            olvaso.Close();
            lekerdezes.CommandText = $"Select name from sellers where id in (select distinct sellerid from realestates) order by name desc;";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read()) 
            {
                Console.WriteLine($"{olvaso.GetString(0)}");
            }
            olvaso.Close();

            kapcsolat.Close();
            Console.ReadKey();
        }
    }
}
