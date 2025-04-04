using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
namespace CUKRASZDA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            var serverKapcsolat = new MySqlConnectionStringBuilder { Server = "127.0.0.1", Database = "cukraszda", UserID = "root", Password = "" };
            MySqlConnection kapcsolat;
            kapcsolat = new MySqlConnection(serverKapcsolat.ConnectionString);
            kapcsolat.Open();
            var lekerdezes = kapcsolat.CreateCommand();
            lekerdezes.CommandText = $"SELECT count(*) AS \"Hiányzó kalória érték\"\r\nfrom termek\r\nWHERE termek.kaloria IS null;";
            var olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetInt16(0)} terméknél nem adtak meg kalóriát.");
            }
            olvaso.Close();
            //4.feladat
            Console.WriteLine();
            byte a = 0;
            lekerdezes.CommandText = $"SELECT termek.nev, kiszereles.mennyiseg\r\nFROM termek INNER JOIN kiszereles ON termek.kiszerelesId = kiszereles.id\r\nWHERE kiszereles.mennyiseg LIKE \"%g\"";
            olvaso = lekerdezes.ExecuteReader();
            while(olvaso.Read()) 
            {
                list.Add(olvaso.GetString(0));
            }
            olvaso.Close();
            lekerdezes.CommandText = $"SELECT  kiszereles.mennyiseg\r\nFROM termek INNER JOIN kiszereles ON termek.kiszerelesId = kiszereles.id\r\nWHERE kiszereles.mennyiseg LIKE \"%g\";";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read()) 
            {
                Console.WriteLine($"Azoknak a termékeknek a neve amelyek \"g\"-re végződnek: {list[a]} , mennyisége: {olvaso.GetString(0)} ");
                a++;
            }
            list.Clear();
            olvaso.Close();
            Console.WriteLine();
            //5.feladat
            lekerdezes.CommandText = $"SELECT allergen.nev, COUNT(*) AS \"termék szám\"\r\nFROM termek INNER JOIN allergeninfo on termek.id = allergeninfo.termekId INNER JOIN allergen ON allergeninfo.allergenId = allergen.id\r\nGROUP BY allergen.nev  \r\nORDER BY `termék szám` DESC\r\nLIMIT 3;";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                list.Add(olvaso.GetString(0));
            }
            olvaso.Close();
            lekerdezes.CommandText = $"SELECT  COUNT(*) AS \"termék szám\"\r\nFROM termek INNER JOIN allergeninfo on termek.id = allergeninfo.termekId INNER JOIN allergen ON allergeninfo.allergenId = allergen.id\r\nGROUP BY allergen.nev  \r\nORDER BY `termék szám` DESC\r\nLIMIT 3;";
            olvaso = lekerdezes.ExecuteReader();
            a = 0;
            while (olvaso.Read())
            {
                Console.WriteLine($"Termék darabszáma: {olvaso.GetInt64(0)} allergén: {list[a]}");
                a++;
            }
            list.Clear();
            olvaso.Close();

            //6.feladat
            Console.WriteLine();
            lekerdezes.CommandText = $"SELECT termek.nev, termek.ar\r\nFROM termek LEFT JOIN allergeninfo ON termek.id = allergeninfo.termekId\r\nWHERE termek.laktozmentes = 1 AND termek.tejmentes = 1 AND termek.tojasmentes = 1 AND allergeninfo.id is null;";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read()) 
            {
                list.Add(olvaso.GetString(0));
            }
            olvaso.Close();
            list.Clear();
            a = 0;
            lekerdezes.CommandText = $"SELECT termek.nev, termek.ar\r\nFROM termek LEFT JOIN allergeninfo ON termek.id = allergeninfo.termekId\r\nWHERE termek.laktozmentes = 1 AND termek.tejmentes = 1 AND termek.tojasmentes = 1 AND allergeninfo.id is null;";
            olvaso = lekerdezes.ExecuteReader();
            while(olvaso.Read()) 
            {
                list.Add(olvaso.GetString(0));
            }
            olvaso.Close();
            lekerdezes.CommandText = $"SELECT termek.ar\r\nFROM termek LEFT JOIN allergeninfo ON termek.id = allergeninfo.termekId\r\nWHERE termek.laktozmentes = 1 AND termek.tejmentes = 1 AND termek.tojasmentes = 1 AND allergeninfo.id is null;";
            olvaso = lekerdezes.ExecuteReader() ;
            while (olvaso.Read()) 
            {

                Console.WriteLine($"A vegán termékek neve {list[a]} és az ára {olvaso.GetInt32(0)}Ft");
                a++;
            }
            olvaso.Close();
            list.Clear();
            a = 0;
            //7.feladat
            lekerdezes.CommandText = $"SELECT termek.nev AS \"torta neve\" , (termek.ar*12-1200) AS \"fizetendő ár\"\r\nFROM termek INNER JOIN kiszereles on termek.kiszerelesId = kiszereles.id\r\nWHERE termek.nev LIKE \"paleo%\";";
            olvaso = lekerdezes.ExecuteReader();
            Console.WriteLine();
            while (olvaso.Read()) 
            {
                Console.WriteLine($"{olvaso.GetString(0)} torta, fizetendő ár: {olvaso.GetInt32(1)}Ft");
            }
            olvaso.Close();
            Console.ReadKey();
        }
    }
}
