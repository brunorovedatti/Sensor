using System;
using System.Net;
using System.Net.Mail;

namespace Utilitarios
{
    public class Sender
    {
        LogFile oLog = new LogFile();

        string fic = "";

        public bool redOSDE;

        public string mMailTo;
        public string mMailCc;
        public string mMailBcc;
        public string mAttachment = "";

        public string mMailFrom = "";
        public string mMailFromName = "";
        public string pass = "";
        public string mHost = "mail.osde.ar";
        public Int32 mPort = 25;

        public string mMsg = "";
        public string mSubject = "";

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
                return "";
        }

        public void Envio()
        {
            try
            {
                //fic = Application.StartupPath.ToString() + "\\System.Sender.Config.dll";
                //string text = System.IO.File.ReadAllText(fic) + "\r\n"; //Esto lo hago porque puede que no deje un enter en el ultimo campo;
                //string strPort = getBetween(text, "[Port]=", "\r");
                SenderConfig oS = new SenderConfig();
                string strPort;
                string strMailFrom;
                string strMailFromName;
                string strHost;
                string strSubject;
                string strPass;
                string strFirma;

                if (redOSDE)
                {
                    strPort = oS.PortOSDE;
                    strMailFrom = oS.MailFromOSDE;
                    strMailFromName = oS.MailFromNameOSDE;
                    strHost = oS.HostOSDE;
                    strSubject = oS.SubjectOSDE;
                    strPass = oS.PassOSDE;
                    strFirma = oS.FirmaOSDE;
                }
                else
                {
                    strPort = oS.Port;
                    strMailFrom = oS.MailFrom;
                    strMailFromName = oS.MailFromName;
                    strHost = oS.Host;
                    strSubject = oS.Subject;
                    strPass = oS.Pass;
                    strFirma = oS.Firma;
                }

                mPort = Convert.ToInt32(strPort);
                mHost = strHost;
                mMailFrom = strMailFrom;
                mMailFromName = strMailFromName;
                if (strPass == "")
                    pass = "";
                else
                    pass = CryptorEngine.Decrypt(strPass, true);
                mSubject = strSubject;

                MailAddress mailTo = new MailAddress(mMailTo);
                MailMessage message = new MailMessage();
                message.To.Add(mailTo);
                if (mMailCc != null)
                {
                    MailAddress mailCc = new MailAddress(mMailCc);
                    message.CC.Add(mailCc);
                }
                if (mMailBcc != null)
                {
                    MailAddress mailBcc = new MailAddress(mMailBcc);
                    message.Bcc.Add(mailBcc);
                }
                MailAddress mailFrom = new MailAddress(mMailFrom, mMailFromName);
                message.From = mailFrom;
                message.Subject = mSubject;
                message.Body = mMsg + "<p></p><p><strong> " + strFirma  + "  </strong></p>";
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                //if (mAttachment != "")
                //{
                //    Attachment attachment = new Attachment(mAttachment);
                //    message.Attachments.Add(attachment);
                //}

                SmtpClient mySmtpClient = new SmtpClient
                {
                    Host = mHost,
                    Port = mPort,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(mailFrom.Address, pass)
                };
                mySmtpClient.Send(message);
            }
            catch (FormatException ex)
            {
                oLog.LogToFile("FormatException: " + ex.ToString());
            }
            catch (SmtpException ex)
            {
                oLog.LogToFile("SmtpException: " + ex.ToString());
            }
            catch (Exception ex)
            {
                oLog.LogToFile("Exception: " + ex.ToString());
            }
        }

    }
}
