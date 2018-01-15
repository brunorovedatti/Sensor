using MySql.Data.MySqlClient;
using System.Configuration;

namespace Controlador
{
    public class DbConexion
    {
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
                return "";
        }

        public static MySqlConnection ObtenerConexion()
        {
            string connectionString = "";
            string strServer = ConfigurationManager.AppSettings.Get("Server");
            string strDataSource = ConfigurationManager.AppSettings.Get("DataSource");
            string strUser = ConfigurationManager.AppSettings.Get("User");
            string strPassword = ConfigurationManager.AppSettings.Get("Password");
            string strPort = ConfigurationManager.AppSettings.Get("Port");

            connectionString = "Server=" + strServer + "; Port=" + strPort + "; Database =" + strDataSource + "; Uid=" + strUser + "; pwd='" + strPassword + "';";

            MySqlConnection conectar = new MySqlConnection(connectionString);

            conectar.Open();
            return conectar;
        }

    }
}
