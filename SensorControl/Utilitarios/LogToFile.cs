using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Utilitarios
{
    public class LogFile
    {
        #region Metodos

        public void LogToFile(string texto)
        {
            try
            {
                string Sistema_Path = Application.StartupPath.ToString();
                string path_file = Sistema_Path + "\\Log.txt";
                string user = Environment.UserName;

                StreamWriter sw = new StreamWriter(path_file, true);
                sw.WriteLine(DateTime.Now + " - User: " + user + "   - V: " + Application.ProductVersion);
                sw.WriteLine(texto);
                sw.WriteLine("----------------------------------------------------------------------------------------------------");
                sw.Close();
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}
