namespace Modelo
{
    public class Usuario
    {
        public string Login { get; set; }
        public string Pswd { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string Activation_Code { get; set; }
        public string Priv_Admin { get; set; }

        public Usuario() { }

        public Usuario(string pLogin, string pPswd, string pName, string pEmail, string pActive, string pActivation_Code, string pPriv_Admin)
        {
            this.Login = pLogin;
            this.Pswd = pPswd;
            this.Name = pName;
            this.Email = pEmail;
            this.Active = pActive;
            this.Activation_Code = pActivation_Code;
            this.Priv_Admin = pPriv_Admin;
        }
    }
}
