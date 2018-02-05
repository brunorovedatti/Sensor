using MySql.Data.MySqlClient;
using System.Configuration;
using Utilitarios;

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
            string strServer = CryptorEngine.Decrypt(ConfigurationManager.AppSettings.Get("Server"), true);
            string strDataSource = CryptorEngine.Decrypt(ConfigurationManager.AppSettings.Get("DataSource"), true);
            string strUser = CryptorEngine.Decrypt(ConfigurationManager.AppSettings.Get("User"), true);
            string strPassword = CryptorEngine.Decrypt(ConfigurationManager.AppSettings.Get("Password"), true);
            string strPort = CryptorEngine.Decrypt(ConfigurationManager.AppSettings.Get("Port"), true);

            connectionString = "Server=" + strServer + "; Port=" + strPort + "; Database =" + strDataSource + "; Uid=" + strUser + "; pwd='" + strPassword + "';";

            MySqlConnection conectar = new MySqlConnection(connectionString);

            conectar.Open();
            return conectar;
        }

    }
}
