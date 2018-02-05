using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    public class SenderConfig
    {
        public string PortOSDE = "25";
        public string MailFromOSDE = "computos_pergamino@osde.com.ar";
        public string MailFromNameOSDE = "Sensor Control";
        public string HostOSDE = "mail.osde.ar";
        public string SubjectOSDE = "Alerta Medicion en Sensor";
        public string PassOSDE = "";
        public string FirmaOSDE = "OSDE FILIAL PERGAMINO";

        public string Port = "26";
        public string MailFrom = "soporte@facturaexpress.com.ar";
        public string MailFromName = "Sensor Control";
        public string Host = "mail.facturaexpress.com.ar";
        public string Subject = "Alerta Medicion en Sensor";
        public string Pass = "5JUm0Mz69kM=";
        public string Firma = "SENSOR";
    }
}
