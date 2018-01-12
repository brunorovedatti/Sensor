using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Controlador
{
    public class VariablesDAL
    {
        public static int ModificarAlertaNotificada(Modelo.Variable pV)
        {
            int retorno = 0;
            string strSQL = "";
            strSQL = strSQL + " UPDATE Variables SET ";
            strSQL = strSQL + "                Alerta_Notificada = " + pV.Alerta_Notificada;
            strSQL = strSQL + " WHERE Id_Variable = " + pV.Id_Variable;
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            retorno = _comando.ExecuteNonQuery();

            MyConn.Close();

            return retorno;
        }
    }
}
