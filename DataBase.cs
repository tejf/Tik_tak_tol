using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Projekt1
{
    /// <summary>
    /// klasa odpowiadająca za połączenie z bazą SQL
    /// </summary>
    class DataBase
    {
        public MySqlConnection Connection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=projekt_kalkulator");
        /// <summary>
        /// Metoda otwierajaca polaczenie z baza danych SQL
        /// </summary>
        public void openConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
        }
        /// <summary>
        /// Metoda zamykajaca polaczenie z baza danych SQL
        /// </summary>
        public void closeConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }
        /// <summary>
        /// Metoda zwracająca MYSqlConnection
        /// </summary>
        /// <returns></returns>
        public MySqlConnection getConnection()
        {
            return Connection;
        }
    }
}
