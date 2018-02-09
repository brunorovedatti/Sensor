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
            strSQL = strSQL + " WHERE Id_Variable = '" + pV.Id_Variable + "'";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            retorno = _comando.ExecuteNonQuery();

            MyConn.Close();

            return retorno;
        }

        public static List<Modelo.Variable> RecuperarAlertaFecha(string id_equipo)
        {
            List<Modelo.Variable> _lista = new List<Modelo.Variable>();
            string strSQL = @"
            SELECT
                alerta_notificada
                , operador_alerta_variable
                , alerta_variable
                , id_variable
                , Nombre_variable
                , unidad_variable
            FROM
                sensor.variables
            WHERE (es_fecha = 1
                AND id_equipo = @idEquipo
                AND estado_variable = 0);
            ";

            MySqlConnection MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(strSQL, MyConn);
            _comando.Parameters.AddWithValue("@idEquipo", id_equipo);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Variable pVariable = new Modelo.Variable();
                pVariable.Alerta_Notificada = _reader.GetBoolean(0);
                pVariable.Operador_Alerta_Variable = _reader.GetString(1);
                pVariable.Alerta_Variable = _reader.GetString(2);
                pVariable.Id_Variable = _reader.GetString(3);
                pVariable.Nombre_Variable = _reader.GetString(4);
                pVariable.Unidad_Variable = _reader.GetString(5);

                _lista.Add(pVariable);
            }

            MyConn.Close();

            return _lista;
        }

    }
}
