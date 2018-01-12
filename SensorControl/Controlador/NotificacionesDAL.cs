using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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

        public static List<Modelo.Notificacion> ValoresMinimos_y_Maximos(string pIdVariable, DateTime pFDesde, DateTime pFHasta)
        {
            List<Modelo.Notificacion> _lista = new List<Modelo.Notificacion>();
            string strSQL = "";
            strSQL = strSQL + "SELECT ";
            strSQL = strSQL + "        DATE_FORMAT(N.fecha_notificacion, '%d/%m/%Y') AS fecha_notificacion ";
            strSQL = strSQL + "      , COUNT(CASE alerta_notificada WHEN 'True' THEN 1 END) AS count_true,  ";
            strSQL = strSQL + "      , COUNT(CASE alerta_notificada WHEN 'False' THEN 1 END) AS count_false,  ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               Notificaciones AS N ";
            strSQL = strSQL + "WHERE ";
            strSQL = strSQL + "         N.id_variable = " + pIdVariable;
            strSQL = strSQL + "     AND DATE_FORMAT(N.fecha_notificacion, '%d/%m/%Y') BETWEEN '" + pFDesde + "' AND '" + pFHasta + "'";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Notificacion pNotificacion = new Modelo.Notificacion();
                pNotificacion.Fecha_Notificacion = _reader.GetString(0);
                pNotificacion.Count_True = _reader.GetInt32(1);
                pNotificacion.Count_False = _reader.GetInt32(2);

                _lista.Add(pNotificacion);
            }

            MyConn.Close();

            return _lista;
        }

    }
}
