namespace Modelo
{
    public class Usuario
    {
        public int Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Contraseña { get; set; }
        public bool Estado_Usuario { get; set; }
        public string Nombre_Completo { get; set; }
        public string Email_Usuario { get; set; }
        public int Id_Conexion { get; set; }
        public Usuario() { }

        public Usuario(int pId_Usuario, string pNombre_Usuario, string pContraseña, bool pEstado_Usuario, string pNombre_Completo, string pEmail_Usuario, int pId_Conexion)
        {
            this.Id_Usuario = pId_Usuario;
            this.Nombre_Usuario = pNombre_Usuario;
            this.Contraseña = pContraseña;
            this.Estado_Usuario = pEstado_Usuario;
            this.Nombre_Completo = pNombre_Completo;
            this.Email_Usuario = pEmail_Usuario;
            this.Id_Conexion = pId_Conexion;
        }
    }
}
