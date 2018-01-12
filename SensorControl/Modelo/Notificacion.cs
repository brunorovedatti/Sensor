namespace Modelo
{
    public class Notificacion
    {
        public int Id_notificacion{ get; set; }
        public string Fecha_Notificacion { get; set; }
        public string Id_Variable { get; set; }
        public string Nombre_Variable { get; set; }
        public string Email_Notificacion { get; set; }
        public string Id_Conexion { get; set; }
        public string Nombre_Conexion { get; set; }
        public string Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Id_Equipo { get; set; }
        public string Nombre_Equipo { get; set; }
        public string Alerta_Notificada { get; set; }
        public string Valor_Variable { get; set; }
        public string Operador_Variable { get; set; }
        public string Valor_Leido { get; set; }
        public Notificacion() { }

        public Notificacion(int pId_notificacion, string pFecha_Notificacion, string pId_Variable, string pNombre_Variable, string pEmail_Notificacion, string pId_Conexion, string pNombre_Conexion, string pId_Usuario, string pNombre_Usuario, string pId_Equipo, string pNombre_Equipo, string pAlerta_Notificada, string pValor_Variable, string pOperador_Variable, string pValor_Leido)
        {
            this.Id_notificacion = pId_notificacion;
            this.Fecha_Notificacion = pFecha_Notificacion;
            this.Id_Variable = pId_Variable;
            this.Nombre_Variable = pNombre_Variable;
            this.Email_Notificacion = pEmail_Notificacion;
            this.Id_Conexion = pId_Conexion;
            this.Nombre_Conexion = pNombre_Conexion;
            this.Id_Usuario = pId_Usuario;
            this.Nombre_Usuario = pNombre_Usuario;
            this.Id_Equipo = pId_Equipo;
            this.Nombre_Equipo = pNombre_Equipo;
            this.Alerta_Notificada = pAlerta_Notificada;
            this.Valor_Variable = pValor_Variable;
            this.Operador_Variable = pOperador_Variable;
            this.Valor_Leido = pValor_Leido;
        }
    }
}
