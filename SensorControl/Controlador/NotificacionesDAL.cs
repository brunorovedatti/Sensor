using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Controlador
{
    public class NotificacionesDAL
    {
        public static int Agregar(Modelo.Notificacion pN)
        {
            int retorno = 0;
            string strSQL = "";
            strSQL = strSQL + " INSERT INTO Notificaciones ";
            strSQL = strSQL + " (Id_Variable, Nombre_Variable, Email_Notificado, Id_Conexion, Nombre_Conexion, Id_Usuario, Nombre_Usuario, Id_Equipo, Nombre_Equipo, Alerta_Notificada, Valor_Variable, Operador_Variable, Valor_Leido, version_sistema ";
            strSQL = strSQL + " ) VALUES ";
            strSQL = strSQL + "('" + pN.Id_Variable + "', '" + pN.Nombre_Variable + "', '" + pN.Email_Notificacion + "', '" + pN.Id_Conexion + "', '" + pN.Nombre_Conexion + "', '"+ pN.Id_Usuario + "', '" + pN.Nombre_Usuario + "', '"+ pN.Id_Equipo + "', '" + pN.Nombre_Equipo + "', '" + pN.Alerta_Notificada + "', '" + pN.Valor_Variable + "', '" + pN.Operador_Variable + "', '" + pN.Valor_Leido + "', '" + Application.ProductVersion + "' ";
            strSQL = strSQL + " )";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            retorno = _comando.ExecuteNonQuery();

            MyConn.Close();

            return retorno;
        }
    }
}
