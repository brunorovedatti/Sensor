namespace Modelo
{
    public class Ip
    {
        public string IP { get; set; }
        public string Ip_User { get; set; }
        public string Isp { get; set; }
        public string Country { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Zona { get; set; }
        public string Ciudad { get; set; }
        public string Continente { get; set; }

        public Ip() { }

        public Ip(string pIP, string pIp_User, string pIsp, string pCountry, string pLatitud, string pLongitud, string pZona, string pCiudad, string pContinente)
        {
            this.IP = pIP;
            this.Ip_User = pIp_User;
            this.Isp = pIsp;
            this.Country = pCountry;
            this.Latitud = pLatitud;
            this.Longitud = pLongitud;
            this.Zona = pZona;
            this.Ciudad = pCiudad;
            this.Continente = pContinente ;
        }
    }
}
