using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Controlador
{
    public class IpsDAL
    {
        public static List<Modelo.Ip> RecuperarTodasLasIps()
        {
            List<Modelo.Ip> _lista = new List<Modelo.Ip>();
            string strSQL = @"
            	SELECT 
                      L.ip_user
                    , IFNULL(ip.ip, '') AS ip
	            FROM
	                (SELECT ip_user FROM sensor.sc_log GROUP BY ip_user) AS L
	            LEFT JOIN
	                (SELECT ip FROM ips GROUP BY ip) AS ip ON ip.ip = L.ip_user
            ";

            MySqlConnection MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(strSQL, MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Ip pIp = new Modelo.Ip();
                pIp.Ip_User = _reader.GetString(0);
                pIp.IP = _reader.GetString(1);

                _lista.Add(pIp);
            }

            MyConn.Close();

            return _lista;
        }

        public static int Agregar(Modelo.Ip pI)
        {
            int retorno = 0;
            string strSQL = "";
            strSQL = strSQL + " INSERT INTO sensor.ips ";
            strSQL = strSQL + " (ip, isp, country, latitud, longitud, zona, ciudad, continente ";
            strSQL = strSQL + " ) VALUES ";
            strSQL = strSQL + "('" + pI.IP + "', '" + pI.Isp + "', '" + pI.Country + "', '" + pI.Latitud + "', '" + pI.Longitud + "', '" + pI.Zona + "', '" + pI.Ciudad + "', '" + pI.Continente + "'";
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
