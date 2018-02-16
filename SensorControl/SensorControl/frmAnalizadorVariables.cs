using System;
using System.Windows.Forms;
using Utilitarios;
using System.Net.NetworkInformation;

namespace SensorControl
{
    public partial class frmAnalizadorVariables : Form
    {
        Sender oSender = new Sender();

        LogFile oLog = new LogFile();

        NotifyIcon NotifyIcon1 = new NotifyIcon();

        public bool redOSDE = false;

        public frmAnalizadorVariables()
        {
            InitializeComponent();
        }

        private void frmAnalizadorVariables_Load(object sender, EventArgs e)
        {
            try
            {
                //Primero voy a hacer PING contra el mail.osde.ar, si responde es porque estoy dentro de la RED de OSDE.
                //Lo hago para definir que servidor de correo debo utilizar automaticamente
                Utilitarios.SenderConfig pingMAIL = new Utilitarios.SenderConfig();
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(pingMAIL.HostOSDE);
                if (reply.Status == IPStatus.Success)
                    redOSDE = true;
            }
            catch (Exception)
            {
                redOSDE = false;
            }
            tmrActualizador.Enabled = true;
            NotifyIcon1.Icon = this.Icon;
            NotifyIcon1.Text = "Control Sensor" + " - v:" + Application.ProductVersion;
            NotifyIcon1.Visible = true;
            ChequearDatos();
            ChequearDesconexion();
            //tmrInicial.Enabled = false;
            
        }

        private void EnvioEmailNotificacionAlerta(Modelo.Lectura oL, string pSin_Conexion_Equipo, string pTipo_Estado)
        {
            foreach (Modelo.Usuario oUsuario in Controlador.UsuariosDAL.BuscarPorConexion(oL.Id_Conexion))
            {
                oSender.mMailTo = oUsuario.Email;
                string e_mail = "";
                string msjs_pedido = "";
                e_mail = "<p> Estimado,     </p><p></p>";
                msjs_pedido = msjs_pedido + "<p> Informamos que el Equipo <strong>" + oL.Nombre_Equipo + "</strong> con Ubicación <strong>" + oL.Nombre_Ubicacion + "</strong> para la Conexión <strong>" + oL.Nombre_Conexion + "</strong>";
                switch (pSin_Conexion_Equipo)
                {
                    case "CONECTADO": //Este caso seria cuando hay una alerta, y el equipo esta conectado
                        switch (pTipo_Estado)
                        {
                            case "NORMAL":
                                msjs_pedido = msjs_pedido + " desactivo una alerta para la variable <strong>" + oL.Nombre_Variable + "</strong>.</p>";
                                msjs_pedido = msjs_pedido + "<p> Valor según medición: <strong>" + oL.Valor_Lectura + " " + oL.Unidad_Variable + "</strong>.</p>";
                                msjs_pedido = msjs_pedido + "<p> Valor sugerido: <strong>" + oL.Operador_Alerta_Variable + " " + oL.Alerta_Variable + oL.Unidad_Variable + "</strong>.</p>";
                                ActualizarAlertaNotificacion(oL, oUsuario, pSin_Conexion_Equipo, pTipo_Estado);
                                break;

                            case "FALLA":
                                msjs_pedido = msjs_pedido + " presento una alerta para la variable <strong>" + oL.Nombre_Variable + "</strong> al no cumplir con lo establecido.</p>";
                                msjs_pedido = msjs_pedido + "<p> Valor según medición: <strong>" + oL.Valor_Lectura + " " + oL.Unidad_Variable + "</strong>.</p>";
                                msjs_pedido = msjs_pedido + "<p> Valor sugerido: <strong>" + oL.Operador_Alerta_Variable + " " + oL.Alerta_Variable + oL.Unidad_Variable + "</strong>.</p>";

                                ActualizarAlertaNotificacion(oL, oUsuario, pSin_Conexion_Equipo, pTipo_Estado);
                                break;
                        }
                        break;
                    case "DESCONECTADO": //Este caso seria cuando el equipo esta desconectado
                        msjs_pedido = msjs_pedido + " no esté conectado en este momento, por lo tanto no reporta eventos. Favor de corroborar que su equipo esté conectado correctamente.</p>";

                        ActualizarAlertaNotificacion(oL, oUsuario, pSin_Conexion_Equipo, pTipo_Estado);
                        break;
                    case "RECONECTADO": //Este caso seria cuando el equipo estaba desconectado y se volvio a conectar
                        msjs_pedido = msjs_pedido + " está nuevamente conectado.</p>";

                        ActualizarAlertaNotificacion(oL, oUsuario, pSin_Conexion_Equipo, pTipo_Estado);
                        break;
                }
                e_mail = e_mail + "</ul>" + msjs_pedido + "</ul>";
                e_mail = e_mail + "<p></p><p>          Muchas gracias.     </p>";
                e_mail = e_mail + "<p></p><p> Saludos,     </p>";
                oSender.mMsg = e_mail;
                oSender.redOSDE = redOSDE;
                oSender.Envio();
            }
        }

        private void ActualizarAlertaNotificacion(Modelo.Lectura oL, Modelo.Usuario oU, string pSin_Conexion_Equipo, string pTipo_Estado)
        {
            int resultado;
            Modelo.Notificacion pNotificacion = new Modelo.Notificacion();
            Modelo.Variable pVariable = new Modelo.Variable();
            Modelo.Equipo pEquipo = new Modelo.Equipo();
            switch (pSin_Conexion_Equipo)
            {
                case "CONECTADO":
                    switch (pTipo_Estado)
                    {
                        case "NORMAL": //Volvio a estar OK la variable, saco la NOTIFICACION, pongo en FALSE, de la VARIABLE
                            pVariable.Id_Variable = oL.Id_Variable;
                            resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                            pVariable.Alerta_Notificada = false;
                            pVariable.Id_Variable = oL.Id_Variable;
                            resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                            pNotificacion.Alerta_Notificada = false.ToString();
                            break;

                        case "FALLA": //Aca presento la falla en una variable, pongo la NOTIFICACION en TRUE de la VARIABLE
                            pVariable.Id_Variable = oL.Id_Variable;
                            resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                            pVariable.Alerta_Notificada = true;
                            pVariable.Id_Variable = oL.Id_Variable;
                            resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                            pNotificacion.Alerta_Notificada = true.ToString();
                            break;
                    }
                    pNotificacion.Valor_Leido = oL.Valor_Lectura;
                    break;
                case "DESCONECTADO": //Este caso seria cuando el equipo esta desconectado

                    pVariable.Alerta_Notificada = true;
                    pVariable.Id_Variable = oL.Id_Variable;
                    resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                    pNotificacion.Valor_Leido = "DESCONECTADO.";
                    pNotificacion.Alerta_Notificada = true.ToString();

                    pEquipo.Sin_Conexion_Equipo = true;
                    pEquipo.Id_Equipo = oL.Id_Equipo;
                    resultado = Controlador.EquiposDAL.ModificarSinConexion(pEquipo);
                    break;
                case "RECONECTADO": //Este caso seria cuando el equipo estaba desconectado y se volvio a conectar

                    pVariable.Alerta_Notificada = false;
                    pVariable.Id_Variable = oL.Id_Variable;
                    resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                    pNotificacion.Valor_Leido = "RECONECTADO.";
                    pNotificacion.Alerta_Notificada = true.ToString();

                    pEquipo.Sin_Conexion_Equipo = false;
                    pEquipo.Id_Equipo = oL.Id_Equipo;
                    resultado = Controlador.EquiposDAL.ModificarSinConexion(pEquipo);
                    break;
            }
            pNotificacion.Id_Variable = oL.Id_Variable;
            pNotificacion.Nombre_Variable = oL.Nombre_Variable;
            pNotificacion.Valor_Variable = oL.Alerta_Variable;
            pNotificacion.Operador_Variable = oL.Operador_Alerta_Variable;

            pNotificacion.Email_Notificacion = oU.Email;
            pNotificacion.Id_Conexion = oL.Id_Conexion.ToString();
            pNotificacion.Nombre_Conexion = oL.Nombre_Conexion;
            pNotificacion.Id_Usuario = oU.Login;
            pNotificacion.Nombre_Usuario = oU.Name;
            pNotificacion.Id_Equipo = oL.Id_Equipo;
            pNotificacion.Nombre_Equipo = oL.Nombre_Equipo;
            resultado = Controlador.NotificacionesDAL.Agregar(pNotificacion);
        }

        private void frmAnalizadorVariables_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void tmrActualizador_Tick(object sender, EventArgs e)
        {
            this.Close();
            ChequearDatos();
            ChequearDesconexion();
        }

        private void ChequearDatos()
        {
            try
            {
                decimal dcmValorLectura;
                decimal dcmAlertaVariable = 0;
                foreach (Modelo.Lectura oLectura in Controlador.LecturasDAL.RecuperarUltimasLecturas())
                {
                    dcmValorLectura = Convert.ToDecimal(oLectura.Valor_Lectura.Replace(".", ","));
                    if (!System.String.IsNullOrEmpty(oLectura.Alerta_Variable))
                        dcmAlertaVariable = Convert.ToDecimal(oLectura.Alerta_Variable.Replace(".", ","));
                    if (!oLectura.Analizada_Lectura)
                    {
                        switch (oLectura.Operador_Alerta_Variable)
                        {
                            case ">":
                                if (dcmValorLectura > dcmAlertaVariable)
                                {//Hay una alerta
                                    if (!oLectura.Alerta_Notificada)//no esta notificada y debo hacerlo
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "FALLA");
                                }
                                else
                                {//No hay alerta, volvio a la normalidad
                                    if (oLectura.Alerta_Notificada)//Si es TRUE significa que se notifico cuando ocurrio la alerta y ahora que esta normal, lo saco de alerta
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "NORMAL"); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                            case ">=":
                                if (dcmValorLectura >= dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "FALLA");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "NORMAL"); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                            case "<":
                                if (dcmValorLectura < dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "FALLA");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "NORMAL"); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                            case "<=":
                                if (dcmValorLectura <= dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "FALLA");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", "NORMAL"); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                                //default:
                                //    Console.WriteLine("Default case");
                                //    break;
                        }
                        int resultado;
                        Modelo.Lectura pLectura = new Modelo.Lectura();
                        pLectura.Id_Lectura = oLectura.Id_Lectura;
                        pLectura.Id_Variable = oLectura.Id_Variable;
                        resultado = Controlador.LecturasDAL.AnalizadadLecturaTrue(pLectura);
                    }
                }
            }
            catch (Exception ex)
            {
                oLog.LogToFile(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }
}

        private void ChequearDesconexion()
        {
            try
            {
                decimal dcmValorLectura;
                decimal dcmAlertaVariable = 0;
                foreach (Modelo.Lectura oL in Controlador.LecturasDAL.RecuperarUltimaNotificacionEnviada())
                {
                    TimeSpan Diferencia_Entre_Fechas = DateTime.Now.Subtract(Convert.ToDateTime(oL.Fecha_Lectura));
                    dcmValorLectura = Convert.ToDecimal(Diferencia_Entre_Fechas.Minutes) + Convert.ToDecimal(Diferencia_Entre_Fechas.Hours) * 60 + Convert.ToDecimal(Diferencia_Entre_Fechas.Days) * 24 * 60;
                    if (!System.String.IsNullOrEmpty(oL.Alerta_Variable))
                        dcmAlertaVariable = Convert.ToDecimal(oL.Alerta_Variable.Replace(".", ","));

                    switch (oL.Operador_Alerta_Variable)
                    {
                        case ">":
                            if (dcmValorLectura > dcmAlertaVariable)
                            {//equipo desconectado
                                if (!oL.Sin_Conexion_Equipo) //Si es TRUE no entra porque ya envie mail. Si es FALSE significa que tengo que notificar.
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", "FALLA");
                            }
                            else
                            {//equipo conectado
                                if (oL.Sin_Conexion_Equipo)// Si es TRUE significa que reconecto, debo notificar. Si es FALSE esta andando bien el equipo y no notifico
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", "NORMAL"); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                        case ">=":
                            if (dcmValorLectura >= dcmAlertaVariable)
                            {
                                if (!oL.Sin_Conexion_Equipo)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", "FALLA");
                            }
                            else
                            {
                                if (!oL.Sin_Conexion_Equipo)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", "NORMAL");  //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                        case "<":
                            if (dcmValorLectura < dcmAlertaVariable)
                            {
                                if (!oL.Sin_Conexion_Equipo)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", "FALLA");
                            }
                            else
                            {
                                if (!oL.Sin_Conexion_Equipo)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", "NORMAL");  //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                        case "<=":
                            if (dcmValorLectura <= dcmAlertaVariable)
                            {
                                if (!oL.Sin_Conexion_Equipo)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", "FALLA");
                            }
                            else
                            {
                                if (!oL.Sin_Conexion_Equipo)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", "NORMAL"); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                            //default:
                            //    Console.WriteLine("Default case");
                            //    break;
                    }

                }
            }
            catch (Exception ex)
            {
                oLog.LogToFile(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }
        }

    }
}
