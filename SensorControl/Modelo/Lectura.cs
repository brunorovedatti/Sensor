using System;

namespace Modelo
{
    public class Lectura
    {
        public int Id_Lectura { get; set; }
        public DateTime Fecha_Lectura { get; set; }
        public string Valor_Lectura { get; set; }
        public string Id_Variable { get; set; }
        public string Nombre_Variable { get; set; }
        public string Unidad_Variable { get; set; }
        public string Alerta_Variable { get; set; }
        public string Operador_Alerta_Variable { get; set; }
        public bool Analizada_Lectura { get; set; }
        public string Id_Equipo { get; set; }
        public string Nombre_Equipo { get; set; }
        public int Id_Ubicacion { get; set; }
        public string Nombre_Ubicacion { get; set; }
        public int Id_Conexion { get; set; }
        public string Nombre_Conexion { get; set; }
        public bool Alerta_Notificada { get; set; }
        public bool Sin_Conexion_Equipo { get; set; }
        public string Valor_Maximo { get; set; }
        public string Valor_Minimo { get; set; }

        public Lectura() { }

        public Lectura(int pId_Lectura, DateTime pFecha_Lectura, string pValor_Lectura, int pId_Ubicacion, string pNombre_Variable, string pUnidad_Variable, string pAlerta_Variable, string pOperador_Alerta_Variable, bool pAnalizada_Lectura, string pId_Equipo, string pNombre_Equipo, string pNombre_Ubicacion, int pId_Conexion, string pNombre_Conexion, bool pAlerta_Notificada, bool pSin_Conexion_Equipo)
        {
            this.Id_Lectura = pId_Lectura;
            this.Fecha_Lectura = pFecha_Lectura;
            this.Valor_Lectura = pValor_Lectura;
            this.Nombre_Variable = pNombre_Variable;
            this.Unidad_Variable = pUnidad_Variable;
            this.Analizada_Lectura = pAnalizada_Lectura;
            this.Alerta_Variable = pAlerta_Variable;
            this.Operador_Alerta_Variable = pOperador_Alerta_Variable;
            this.Id_Equipo = pId_Equipo;
            this.Nombre_Equipo = pNombre_Equipo;
            this.Id_Ubicacion = pId_Ubicacion;
            this.Nombre_Ubicacion = pNombre_Ubicacion;
            this.Id_Conexion = pId_Conexion;
            this.Nombre_Conexion = pNombre_Conexion;
            this.Alerta_Notificada = pAlerta_Notificada;
            this.Sin_Conexion_Equipo = pSin_Conexion_Equipo;
            this.Valor_Maximo = pValor_Maximo;
            this.Valor_Minimo = pValor_Minimo;
        }
    }
}
