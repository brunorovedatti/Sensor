namespace Modelo
{
    public class Ubicacion
    {
        public int Id_Ubicacion { get; set; }
        public string Nombre_Ubicacion { get; set; }
        public string Descripcion_Ubicacion { get; set; }
        public bool Estado_Ubicacion { get; set; }

        public Ubicacion() { }

        public Ubicacion(int pId_Ubicacion, string pNombre_Ubicacion, string pDescripcion_Ubicacion, bool pEstado_Ubicacion)
        {
            this.Id_Ubicacion = pId_Ubicacion;
            this.Nombre_Ubicacion = pNombre_Ubicacion;
            this.Descripcion_Ubicacion = pDescripcion_Ubicacion;
            this.Estado_Ubicacion = pEstado_Ubicacion;
        }
    }
}
