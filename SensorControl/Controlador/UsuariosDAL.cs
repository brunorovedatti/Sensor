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
            strSQL = strSQL + "        U.Login ";
            strSQL = strSQL + "      , U.Pswd ";
            strSQL = strSQL + "      , U.Name ";
            strSQL = strSQL + "      , U.Email ";
            strSQL = strSQL + "      , U.Active ";
            strSQL = strSQL + "      , U.Activation_Code ";
            strSQL = strSQL + "      , U.Priv_Admin ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               sensor.usuarios_conexiones AS UC ";
            strSQL = strSQL + "    INNER JOIN sensor.sec_users AS U ";
            strSQL = strSQL + "                                    ON(UC.id_usuario = U.login) ";
            strSQL = strSQL + "WHERE   ( U.Active = 'Y' ";
            strSQL = strSQL + "     AND UC.id_conexion = '" + pIdConexion + "' AND UC.notifico_por_email = 1) ";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Usuario pUsuario = new Modelo.Usuario();
                pUsuario.Login = _reader.GetString(0);
                pUsuario.Pswd = _reader.GetString(1);
                pUsuario.Name = _reader.GetString(2);
                pUsuario.Email = _reader.GetString(3);
                pUsuario.Active = _reader.GetString(4);
                pUsuario.Activation_Code = _reader.GetString(5);
                pUsuario.Priv_Admin = _reader.GetString(6);

                _lista.Add(pUsuario);
            }

            MyConn.Close();

            return _lista;
        }

        public static List<Modelo.Usuario> BuscarPrivAdmin()
        {
            List<Modelo.Usuario> _lista = new List<Modelo.Usuario>();
            string strSQL = "";
            strSQL = strSQL + "SELECT ";
            strSQL = strSQL + "        U.Login ";
            strSQL = strSQL + "      , U.Pswd ";
            strSQL = strSQL + "      , U.Name ";
            strSQL = strSQL + "      , U.Email ";
            strSQL = strSQL + "      , U.Active ";
            strSQL = strSQL + "      , U.Activation_Code ";
            strSQL = strSQL + "      , U.Priv_Admin ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               sensor.sec_users AS U ";
            strSQL = strSQL + "WHERE   ( U.Active = 'Y' AND priv_admin = 'Y')";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Usuario pUsuario = new Modelo.Usuario();
                pUsuario.Login = _reader.GetString(0);
                pUsuario.Pswd = _reader.GetString(1);
                pUsuario.Name = _reader.GetString(2);
                pUsuario.Email = _reader.GetString(3);
                pUsuario.Active = _reader.GetString(4);
                pUsuario.Activation_Code = _reader.GetString(5);
                pUsuario.Priv_Admin = _reader.GetString(6);

                _lista.Add(pUsuario);
            }

            MyConn.Close();

            return _lista;
        }

    }
}
