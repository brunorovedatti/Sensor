namespace Modelo
{
    public class Variable
    {
        public string Id_Variable { get; set; }
        public string Nombre_Variable { get; set; }
        public string Descripcion_Variable { get; set; }
        public string Unidad_Variable { get; set; }
        public bool Estado_Variable { get; set; }
        public string Id_Equipo { get; set; }
        public string Alerta_Variable { get; set; }
        public string Operador_Alerta_Variable { get; set; }
        public bool Alerta_Notificada { get; set; }
        public bool Es_Fecha { get; set; }
        public bool Graficable { get; set; }

        public Variable() { }

        public Variable(string pId_Variable, string pNombre_Variable, string pDescripcion_Variable, string pUnidad_Variable, bool pEstado_Variable, string pId_Equipo, string pAlerta_Variable, string pOperador_Alerta_Variable, bool pAlerta_Notificada, bool pEs_Fecha, bool pGraficable)
        {
            this.Id_Variable = pId_Variable;
            this.Nombre_Variable = pNombre_Variable;
            this.Descripcion_Variable = pDescripcion_Variable;
            this.Unidad_Variable = pUnidad_Variable;
            this.Estado_Variable = pEstado_Variable;
            this.Id_Equipo = pId_Equipo;
            this.Alerta_Variable = pAlerta_Variable;
            this.Operador_Alerta_Variable = pOperador_Alerta_Variable;
            this.Alerta_Notificada = pAlerta_Notificada;
            this.Es_Fecha = pEs_Fecha;
            this.Graficable = pGraficable;
        }
    }
}
