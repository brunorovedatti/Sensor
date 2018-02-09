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
            //tmrInicial.Enabled = false;
            
        }

        private void EnvioEmailNotificacionAlerta(Modelo.Lectura oL, string pSin_Conexion_Equipo, bool pNotificado_Estado)
        {
            foreach (Modelo.Usuario oUsuario in Controlador.UsuariosDAL.BuscarPorConexion(oL.Id_Conexion))
            {
                oSender.mMailTo = oUsuario.Email;
                string e_mail = "";
                string msjs_pedido = "";
                e_mail = "<p> Hola,     </p><p></p>";
                msjs_pedido = msjs_pedido + "<p> El Equipo: " + oL.Nombre_Equipo + " con Ubicacion: " + oL.Nombre_Ubicacion + " para la Conexion: " + oL.Nombre_Conexion + "";
                switch (pSin_Conexion_Equipo)
                {
                    case "CONECTADO": //Este caso seria cuando hay una alerta, y el equipo esta conectado
                        msjs_pedido = msjs_pedido + " presento una alerta para la Variable: " + oL.Nombre_Variable + " al no cumplir con lo establecido.</p>";
                        msjs_pedido = msjs_pedido + "<p> Valor leido: " + oL.Valor_Lectura + " " + oL.Unidad_Variable + " cuando no deberia ser " + oL.Operador_Alerta_Variable + " " + oL.Alerta_Variable + oL.Unidad_Variable + "</p>";

                        ActualizarAlertaNotificacion(oL, true, oUsuario.Email, oUsuario.Login, oUsuario.Name, false, pNotificado_Estado);
                        break;
                    case "DESCONECTADO": //Este caso seria cuando el equipo esta desconectado
                        msjs_pedido = msjs_pedido + " no esta reportando eventos, favor de chequear su conexión.</p>";

                        ActualizarAlertaNotificacion(oL, true, oUsuario.Email, oUsuario.Login, oUsuario.Name, true, pNotificado_Estado);
                        break;
                    case "RECONECTADO": //Este caso seria cuando el equipo estaba desconectado y se volvio a conectar
                        msjs_pedido = msjs_pedido + " esta reportando eventos nuevamente.</p>";

                        ActualizarAlertaNotificacion(oL, false, oUsuario.Email, oUsuario.Login, oUsuario.Name, true, pNotificado_Estado);
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
        
        private void ActualizarAlertaNotificacion(Modelo.Lectura oL, bool pEstado, string email, string id_usuario, string nombre_usuario, bool pSin_Conexion_Equipo, bool pNotificado_Estado)
        {
            int resultado;
            Modelo.Notificacion pNotificacion = new Modelo.Notificacion();
            if (!pSin_Conexion_Equipo)
            {//El quipo siempre esta conectado dentro del IF
                //actualizo la variable en la BD para no seguir notificando
                Modelo.Variable pVariable = new Modelo.Variable();
                pVariable.Alerta_Notificada = pEstado;
                pVariable.Id_Variable = oL.Id_Variable;
                resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);
                pNotificacion.Id_Variable = oL.Id_Variable;
                pNotificacion.Nombre_Variable = oL.Nombre_Variable;
                pNotificacion.Valor_Variable = oL.Alerta_Variable;
                pNotificacion.Operador_Variable = oL.Operador_Alerta_Variable;
                pNotificacion.Valor_Leido = oL.Valor_Lectura;
                pNotificacion.Alerta_Notificada = pEstado.ToString();

                pNotificacion.Email_Notificacion = email;
                pNotificacion.Id_Conexion = oL.Id_Conexion.ToString();
                pNotificacion.Nombre_Conexion = oL.Nombre_Conexion;
                pNotificacion.Id_Usuario = id_usuario;
                pNotificacion.Nombre_Usuario = nombre_usuario;
                pNotificacion.Id_Equipo = oL.Id_Equipo;
                pNotificacion.Nombre_Equipo = oL.Nombre_Equipo;
                resultado = Controlador.NotificacionesDAL.Agregar(pNotificacion);
            }
            if (pSin_Conexion_Equipo)//Actualizo el EQUIPO el campo SIN_CONEXION_EQUIPO
            {//DESCONECTADO o RECONECTADO
                if (!pNotificado_Estado) //No esta notificada la aleerta, debo enviar un mail y guardar un registro
                {
                    //actualizo SIN_CONEXION_EQUIPO en la BD para no seguir notificando
                    Modelo.Equipo pEquipo = new Modelo.Equipo();
                    pEquipo.Sin_Conexion_Equipo = pEstado;
                    pEquipo.Id_Equipo = oL.Id_Equipo;
                    pEquipo.Notificado_Estado = false;
                    resultado = Controlador.EquiposDAL.ModificarSinConexion(pEquipo);

                    Modelo.Variable pVariable = new Modelo.Variable();
                    pVariable.Alerta_Notificada = pEstado;
                    pVariable.Id_Variable = oL.Id_Variable;
                    resultado = Controlador.VariablesDAL.ModificarAlertaNotificada(pVariable);

                    pNotificacion.Id_Variable = "";
                    pNotificacion.Nombre_Variable = "";
                    pNotificacion.Valor_Variable = "";
                    pNotificacion.Operador_Variable = "";
                    pNotificacion.Alerta_Notificada = "";
                    if (pEstado)
                        pNotificacion.Valor_Leido = "EQUIPO DESCONECTADO.";
                    else
                        pNotificacion.Valor_Leido = "EQUIPO RECONECTADO.";

                    pNotificacion.Email_Notificacion = email;
                    pNotificacion.Id_Conexion = oL.Id_Conexion.ToString();
                    pNotificacion.Nombre_Conexion = oL.Nombre_Conexion;
                    pNotificacion.Id_Usuario = id_usuario;
                    pNotificacion.Nombre_Usuario = nombre_usuario;
                    pNotificacion.Id_Equipo = oL.Id_Equipo;
                    pNotificacion.Nombre_Equipo = oL.Nombre_Equipo;
                    resultado = Controlador.NotificacionesDAL.Agregar(pNotificacion);
                }
            }

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
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", true);
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false, true);
                                }
                                break;
                            case ">=":
                                if (dcmValorLectura >= dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", true);
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false, true);
                                }
                                break;
                            case "<":
                                if (dcmValorLectura < dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", true);
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false, true);
                                }
                                break;
                            case "<=":
                                if (dcmValorLectura <= dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO", true);
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false, true);
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
                //Voy a chequear el estado de las variables SIN_CONEXION_EQUIPO y NOTIDICADO_ESTADO del EQUIPO
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
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", oL.Notificado_Estado);
                            }
                            else
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", oL.Notificado_Estado); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                        case ">=":
                            if (dcmValorLectura >= dcmAlertaVariable)
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", oL.Notificado_Estado);
                            }
                            else
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", oL.Notificado_Estado); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                        case "<":
                            if (dcmValorLectura < dcmAlertaVariable)
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", oL.Notificado_Estado);
                            }
                            else
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", oL.Notificado_Estado); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                            }
                            break;
                        case "<=":
                            if (dcmValorLectura <= dcmAlertaVariable)
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "DESCONECTADO", oL.Notificado_Estado);
                            }
                            else
                            {
                                if (!oL.Notificado_Estado)
                                    EnvioEmailNotificacionAlerta(oL, "RECONECTADO", oL.Notificado_Estado); //ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
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
