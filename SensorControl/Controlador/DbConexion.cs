using MySql.Data.MySqlClient;
using System.Windows.Forms;

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
            string fic = "";
            fic = Application.StartupPath.ToString() + "\\MySqlConnector.ConnectionString.dll";
            string text = System.IO.File.ReadAllText(fic) + "\r\n"; //Esto lo hago porque puede que no deje un enter en el ultimo campo;
            string strServer = getBetween(text, "[Server]=", "\r");
            string strDataSource = getBetween(text, "[DataSource]=", "\r");
            string strUser = getBetween(text, "[User]=", "\r");
            string strPassword = getBetween(text, "[Password]=", "\r");
            string strPort = getBetween(text, "[Port]=", "\r");
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
