using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Controlador
{
    public class UsuariosDAL
    {
        public static List<Modelo.Usuario> BuscarPorConexion(int pIdConexion)
        {
            List<Modelo.Usuario> _lista = new List<Modelo.Usuario>();
            string strSQL = "";
            strSQL = strSQL + "SELECT ";
            strSQL = strSQL + "      U.id_usuario ";
            strSQL = strSQL + "      , U.nombre_usuario ";
            strSQL = strSQL + "      , U.nombre_completo ";
            strSQL = strSQL + "      , U.email_usuario ";
            strSQL = strSQL + "      , UC.id_conexion ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               sensor.usuarios_conexiones AS UC ";
            strSQL = strSQL + "    INNER JOIN sensor.usuarios AS U ";
            strSQL = strSQL + "                                    ON(UC.id_usuario = U.id_usuario) ";
            strSQL = strSQL + "WHERE   ( U.estado_usuario = 0 ";
            strSQL = strSQL + "     AND UC.id_conexion = '" + pIdConexion + "') ";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Usuario pUsuario = new Modelo.Usuario();
                pUsuario.Id_Usuario = _reader.GetInt32(0);
                pUsuario.Nombre_Usuario = _reader.GetString(1);
                pUsuario.Nombre_Completo = _reader.GetString(2);
                pUsuario.Email_Usuario = _reader.GetString(3);
                pUsuario.Id_Conexion = _reader.GetInt32(4);

                _lista.Add(pUsuario);
            }

            MyConn.Close();

            return _lista;
        }
    }
}
