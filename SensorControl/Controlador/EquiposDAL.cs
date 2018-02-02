using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Controlador
{
    public class EquiposDAL
    {
        public static List<Modelo.Equipo> Buscar(int pIdEstado, int pExcluirVPN)
        {
            List<Modelo.Equipo> _lista = new List<Modelo.Equipo>();
            string strSQL = "";
            strSQL = strSQL + "SELECT *";
            strSQL = strSQL + " FROM Equipos AS E ";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Equipo pEquipo = new Modelo.Equipo();
                pEquipo.Id_Equipo = _reader.GetString(0);

                _lista.Add(pEquipo);
            }

            MyConn.Close();

            return _lista;
        }

        public static int ModificarSinConexion(Modelo.Equipo pE)
        {
            int retorno = 0;
            string strSQL = "";
            strSQL = strSQL + " UPDATE Equipos SET ";
            strSQL = strSQL + "                Sin_Conexion_Equipo = " + pE.Sin_Conexion_Equipo;
            strSQL = strSQL + " WHERE Id_Equipo = '" + pE.Id_Equipo + "'";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            retorno = _comando.ExecuteNonQuery();

            MyConn.Close();

            return retorno;
        }
    }
}
