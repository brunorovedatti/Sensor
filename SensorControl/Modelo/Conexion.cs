namespace Modelo
{
    public class Conexion
    {
        public int Id_Conexion { get; set; }
        public string Nombre_Conexion { get; set; }
        public bool Estado_Conexion { get; set; }

        public Conexion() { }

        public Conexion(int pId_Conexion, string pNombre_Conexion, bool pEstado_Conexion)
        {
            this.Id_Conexion = pId_Conexion;
            this.Nombre_Conexion = pNombre_Conexion;
            this.Estado_Conexion = pEstado_Conexion;
        }
    }
}
