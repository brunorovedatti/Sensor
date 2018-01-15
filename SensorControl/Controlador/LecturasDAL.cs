using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Controlador
{
    public class LecturasDAL
    {
        public static List<Modelo.Lectura> Buscar(int pIdEstado)
        {
            List<Modelo.Lectura> _lista = new List<Modelo.Lectura>();
            string strSQL = "";
            strSQL = strSQL + "SELECT ";
            strSQL = strSQL + "      L.id_lectura ";
            strSQL = strSQL + "      , L.fecha_lectura";
            strSQL = strSQL + "      , L.valor_lectura ";
            strSQL = strSQL + "      , L.id_variable ";
            strSQL = strSQL + "      , L.analizada_lectura ";
            strSQL = strSQL + "      , V.nombre_variable ";
            strSQL = strSQL + "      , V.unidad_variable ";
            strSQL = strSQL + "      , V.alerta_variable ";
            strSQL = strSQL + "      , V.operador_alerta_variable ";
            strSQL = strSQL + "      , V.id_equipo ";
            strSQL = strSQL + "      , E.nombre_equipo ";
            strSQL = strSQL + "      , E.id_ubicacion ";
            strSQL = strSQL + "      , U.nombre_ubicacion ";
            strSQL = strSQL + "      , E.id_conexion ";
            strSQL = strSQL + "      , C.nombre_conexion ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               sensor.lecturas AS L ";
            strSQL = strSQL + "    INNER JOIN sensor.variables AS V ";
            strSQL = strSQL + "                                    ON(L.id_variable = V.id_variable) ";
            strSQL = strSQL + "    INNER JOIN sensor.equipos AS E ";
            strSQL = strSQL + "                                    ON(V.id_equipo = E.id_equipo) ";
            strSQL = strSQL + "    INNER JOIN sensor.ubicaciones AS U ";
            strSQL = strSQL + "                                    ON(E.id_ubicacion = U.id_ubicacion) ";
            strSQL = strSQL + "    INNER JOIN sensor.conexiones AS C ";
            strSQL = strSQL + "                                    ON(E.id_conexion = C.id_conexion) ";
            strSQL = strSQL + "WHERE ";
            strSQL = strSQL + "     L.analizada_lectura = " + pIdEstado;
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Lectura pLectura = new Modelo.Lectura();
                pLectura.Id_Lectura = _reader.GetInt32(0);
                pLectura.Fecha_Lectura = _reader.GetDateTime(1);
                pLectura.Valor_Lectura = _reader.GetString(2);
                pLectura.Id_Variable = _reader.GetString(3);
                pLectura.Analizada_Lectura = _reader.GetBoolean(4);
                pLectura.Nombre_Variable = _reader.GetString(5);
                pLectura.Unidad_Variable = _reader.GetString(6);
                pLectura.Alerta_Variable = _reader.GetString(7);
                pLectura.Operador_Alerta_Variable = _reader.GetString(8);
                pLectura.Id_Equipo = _reader.GetString(9);
                pLectura.Nombre_Equipo = _reader.GetString(10);
                pLectura.Id_Ubicacion = _reader.GetInt32(11);
                pLectura.Nombre_Ubicacion = _reader.GetString(12);
                pLectura.Id_Conexion = _reader.GetInt32(13);
                pLectura.Nombre_Conexion = _reader.GetString(14);

                _lista.Add(pLectura);
            }

            MyConn.Close();

            return _lista;
        }

        public static List<Modelo.Lectura> RecuperarUltimasLecturas()
        {
            List<Modelo.Lectura> _lista = new List<Modelo.Lectura>();
            string strSQL = @"
            SELECT 
                    L.id_lectura 
                  , L.fecha_lectura
                  , L.valor_lectura 
                  , L.id_variable 
                  , L.analizada_lectura 
                  , V.nombre_variable 
                  , V.unidad_variable 
                  , V.alerta_variable 
                  , V.operador_alerta_variable 
                  , V.id_equipo 
                  , E.nombre_equipo 
                  , E.id_ubicacion 
                  , U.nombre_ubicacion 
                  , E.id_conexion 
                  , C.nombre_conexion 
                  , V.Alerta_Notificada 
                  , E.Sin_Conexion_Equipo 
            FROM sensor.lecturas AS L 
            INNER JOIN sensor.variables AS V 
                ON(L.id_variable = V.id_variable) 
            INNER JOIN sensor.equipos AS E 
                ON(V.id_equipo = E.id_equipo) 
            INNER JOIN sensor.ubicaciones AS U 
                ON(E.id_ubicacion = U.id_ubicacion) 
            INNER JOIN sensor.conexiones AS C 
                ON(E.id_conexion = C.id_conexion)
            INNER JOIN (SELECT  MAX(L.id_lectura) AS id_lectura, L.id_variable FROM lecturas AS L GROUP BY L.id_variable) AS sql_ 
                ON sql_.id_lectura = L.id_lectura 
            WHERE L.analizada_lectura = 0
            ";

            MySqlConnection MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(strSQL, MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Lectura pLectura = new Modelo.Lectura();
                pLectura.Id_Lectura = _reader.GetInt32(0);
                pLectura.Fecha_Lectura = _reader.GetDateTime(1);
                pLectura.Valor_Lectura = _reader.GetString(2);
                pLectura.Id_Variable = _reader.GetString(3);
                pLectura.Analizada_Lectura = _reader.GetBoolean(4);
                pLectura.Nombre_Variable = _reader.GetString(5);
                pLectura.Unidad_Variable = _reader.GetString(6);
                pLectura.Alerta_Variable = _reader.GetString(7);
                pLectura.Operador_Alerta_Variable = _reader.GetString(8);
                pLectura.Id_Equipo = _reader.GetString(9);
                pLectura.Nombre_Equipo = _reader.GetString(10);
                pLectura.Id_Ubicacion = _reader.GetInt32(11);
                pLectura.Nombre_Ubicacion = _reader.GetString(12);
                pLectura.Id_Conexion = _reader.GetInt32(13);
                pLectura.Nombre_Conexion = _reader.GetString(14);
                pLectura.Alerta_Notificada = _reader.GetBoolean(15);
                pLectura.Sin_Conexion_Equipo = _reader.GetBoolean(16);

                _lista.Add(pLectura);
            }

            MyConn.Close();

            return _lista;
        }

        public static List<Modelo.Lectura> RecuperarLecturaGrafico(string id)
        {
            List<Modelo.Lectura> _lista = new List<Modelo.Lectura>();
            string strSQL = @"
            SELECT 
                    L.id_lectura 
                  , L.fecha_lectura
                  , L.valor_lectura 
                  , L.id_variable 
                  , L.analizada_lectura 
                  , V.nombre_variable 
                  , V.unidad_variable 
                  , V.alerta_variable 
                  , V.operador_alerta_variable 
                  , V.id_equipo 
                  , E.nombre_equipo 
                  , E.id_ubicacion 
                  , U.nombre_ubicacion 
                  , E.id_conexion 
                  , C.nombre_conexion 
                  , V.Alerta_Notificada 
                  , E.Sin_Conexion_Equipo 
            FROM sensor.lecturas AS L 
            INNER JOIN sensor.variables AS V 
                ON(L.id_variable = V.id_variable) 
            INNER JOIN sensor.equipos AS E 
                ON(V.id_equipo = E.id_equipo) 
            INNER JOIN sensor.ubicaciones AS U 
                ON(E.id_ubicacion = U.id_ubicacion) 
            INNER JOIN sensor.conexiones AS C 
                ON(E.id_conexion = C.id_conexion)
            WHERE V.id_equipo = @idEquipo
            ORDER BY L.fecha_lectura DESC
            LIMIT 100
            ";

            MySqlConnection MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(strSQL, MyConn);
            _comando.Parameters.AddWithValue("@idEquipo", id);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Lectura pLectura = new Modelo.Lectura();
                pLectura.Id_Lectura = _reader.GetInt32(0);
                pLectura.Fecha_Lectura = _reader.GetDateTime(1);
                pLectura.Valor_Lectura = _reader.GetString(2);
                pLectura.Id_Variable = _reader.GetString(3);
                pLectura.Analizada_Lectura = _reader.GetBoolean(4);
                pLectura.Nombre_Variable = _reader.GetString(5);
                pLectura.Unidad_Variable = _reader.GetString(6);
                pLectura.Alerta_Variable = _reader.GetString(7);
                pLectura.Operador_Alerta_Variable = _reader.GetString(8);
                pLectura.Id_Equipo = _reader.GetString(9);
                pLectura.Nombre_Equipo = _reader.GetString(10);
                pLectura.Id_Ubicacion = _reader.GetInt32(11);
                pLectura.Nombre_Ubicacion = _reader.GetString(12);
                pLectura.Id_Conexion = _reader.GetInt32(13);
                pLectura.Nombre_Conexion = _reader.GetString(14);
                pLectura.Alerta_Notificada = _reader.GetBoolean(15);
                pLectura.Sin_Conexion_Equipo = _reader.GetBoolean(16);

                _lista.Add(pLectura);
            }

            MyConn.Close();

            return _lista;
        }

        public static int AnalizadadLecturaTrue(Modelo.Lectura pL)
        {
            int retorno = 0;
            string strSQL = "";
            strSQL = strSQL + " UPDATE Lecturas SET ";
            strSQL = strSQL + "                Analizada_Lectura = 1";
            strSQL = strSQL + " WHERE Id_Variable = " + pL.Id_Variable + " AND Id_Lectura <= " + pL.Id_Lectura;
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            retorno = _comando.ExecuteNonQuery();

            MyConn.Close();

            return retorno;
        }

        public static List<Modelo.Lectura> BuscarParaCompletarGrilla(string pIdVariable, DateTime pFDesde, DateTime pFHasta)
        {
            List<Modelo.Lectura> _lista = new List<Modelo.Lectura>();
            string strSQL = "";
            strSQL = strSQL + "SELECT ";
            strSQL = strSQL + "        L.fecha_lectura ";
            strSQL = strSQL + "      , L.valor_lectura ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               sensor.lecturas AS L ";
            strSQL = strSQL + "WHERE ";
            strSQL = strSQL + "         L.id_variable = " + pIdVariable;
            strSQL = strSQL + "     AND DATE_FORMAT(L.fecha_lectura, '%d/%m/%Y %H:%i:%S') BETWEEN '" + pFDesde + "' AND '" + pFHasta + "'";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Lectura pLectura = new Modelo.Lectura();
                pLectura.Fecha_Lectura = _reader.GetDateTime(0);
                pLectura.Valor_Lectura = _reader.GetString(1);

                _lista.Add(pLectura);
            }

            MyConn.Close();

            return _lista;
        }

        public static List<Modelo.Lectura> ValoresMinimos_y_Maximos(string pIdVariable, DateTime pFDesde, DateTime pFHasta)
        {
            List<Modelo.Lectura> _lista = new List<Modelo.Lectura>();
            string strSQL = "";
            strSQL = strSQL + "SELECT ";
            strSQL = strSQL + "        L.fecha_lectura";
            strSQL = strSQL + "      , MAX(L.valor_lectura) AS valor_maximo ";
            strSQL = strSQL + "      , MIN(L.valor_lectura) AS valor_minimo ";
            strSQL = strSQL + "FROM ";
            strSQL = strSQL + "               sensor.lecturas AS L ";
            strSQL = strSQL + "WHERE ";
            strSQL = strSQL + "         L.id_variable = " + pIdVariable;
            strSQL = strSQL + "     AND DATE_FORMAT(L.fecha_lectura, '%d/%m/%Y') BETWEEN '" + pFDesde + "' AND '" + pFHasta + "'";
            MySqlConnection MyConn = new MySqlConnection();
            MyConn = DbConexion.ObtenerConexion();
            MySqlCommand _comando = new MySqlCommand(String.Format(strSQL), MyConn);

            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                Modelo.Lectura pLectura = new Modelo.Lectura();
                pLectura.Fecha_Lectura = _reader.GetDateTime(0);
                pLectura.Valor_Maximo = _reader.GetString(1);
                pLectura.Valor_Minimo = _reader.GetString(2);

                _lista.Add(pLectura);
            }

            MyConn.Close();

            return _lista;
        }

    }
}
