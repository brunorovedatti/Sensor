namespace Modelo
{
    public class Equipo
    {
        public string Id_Equipo { get; set; }
        public string Nombre_Equipo { get; set; }
        public bool Estado_Equipo { get; set; }
        public int Id_Ubicacion { get; set; }
        public string Nombre_Ubicacion { get; set; }
        public int Id_Conexion { get; set; }
        public string Nombre_Conexion { get; set; }
        public bool Sin_Conexion_Equipo { get; set; }
        public string Ruta_Actualizacion { get; set; }
        public bool Notificado_Estado { get; set; }

        public Equipo() { }

        public Equipo(string pId_Equipo, string pNombre_Equipo, bool pEstado_Equipo, int pId_Ubicacion, string pNombre_Ubicacion, int pId_Conexion, string pNombre_Conexion, bool pSin_Conexion_Equipo, string pRuta_Actualizacion, bool pNotificado_Estado)
        {
            this.Id_Equipo = pId_Equipo;
            this.Nombre_Equipo = pNombre_Equipo;
            this.Estado_Equipo = pEstado_Equipo;
            this.Nombre_Ubicacion = pNombre_Ubicacion;
            this.Id_Conexion = pId_Conexion;
            this.Nombre_Conexion = pNombre_Conexion;
            this.Sin_Conexion_Equipo = pSin_Conexion_Equipo;
            this.Ruta_Actualizacion = pRuta_Actualizacion;
            this.Notificado_Estado = pNotificado_Estado;
        }
    }
}
