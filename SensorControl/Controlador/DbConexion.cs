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
            string strServer = ConfigurationManager.AppSettings.Get("Server");
            string strDataSource = ConfigurationManager.AppSettings.Get("DataSource");
            string strUser = ConfigurationManager.AppSettings.Get("User");
            string strPassword = ConfigurationManager.AppSettings.Get("Password");
            string strPort = ConfigurationManager.AppSettings.Get("Port");
            string server;
            string dataSource;
            string user;
            string password;
            string port;
            string connectionString = "";

            if (strServer == "")
                server = "";
            else
                server = strServer;

            if (strDataSource == "")
                dataSource = "";
            else
                dataSource = strDataSource;

            if (strUser == "")
                user = "sensor_write";
            else
                user = strUser;

            if (strPassword == "")
                password = "r00t&4ss";
            else
                password = Utilidades.CryptorEngine.Decrypt(strPassword, true);

            if (strPort == "")
                port = "";
            else
                port = strPort;

            connectionString = "Server=" + server + "; Port=" + port + "; Database =" + dataSource + "; Uid=" + user + "; pwd='" + password + "';" ;

            MySqlConnection conectar = new MySqlConnection(connectionString);

            conectar.Open();
            return conectar;
        }

    }
}
