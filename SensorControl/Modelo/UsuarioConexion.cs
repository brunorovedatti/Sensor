namespace Modelo
{
    public class UsuarioConexion
    {
        public int Id_Usuario_Conexion { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Conexion { get; set; }

        public UsuarioConexion() { }

        public UsuarioConexion(int pId_Usuario_Conexion, int pId_Usuario, int pId_Conexion)
        {
            this.Id_Usuario_Conexion = pId_Usuario_Conexion;
            this.Id_Usuario = pId_Usuario;
            this.Id_Conexion = pId_Conexion;
        }
    }
}
