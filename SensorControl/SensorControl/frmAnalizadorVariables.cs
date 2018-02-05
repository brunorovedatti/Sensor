﻿using System;
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

        private void EnvioEmailNotificacionAlerta(Modelo.Lectura oL, string pSin_Conexion_Equipo)
        {
            foreach (Modelo.Usuario oUsuario in Controlador.UsuariosDAL.BuscarPorConexion(oL.Id_Conexion))
            {
                oSender.mMailTo = oUsuario.Email_Usuario;
                string e_mail = "";
                string msjs_pedido = "";
                e_mail = "<p> Hola,     </p><p></p>";
                msjs_pedido = msjs_pedido + "<p> El Equipo: " + oL.Nombre_Equipo + " con Ubicacion: " + oL.Nombre_Ubicacion + " para la Conexion: " + oL.Nombre_Conexion + "";
                switch (pSin_Conexion_Equipo)
                {
                    case "CONECTADO": //Este caso seria cuando hay una alerta, y el equipo esta conectado
                        msjs_pedido = msjs_pedido + " presento una alerta para la Variable: " + oL.Nombre_Variable + " al no cumplir con lo establecido.</p>";
                        msjs_pedido = msjs_pedido + "<p> Valor leido: " + oL.Valor_Lectura + " " + oL.Unidad_Variable + " cuando no deberia ser " + oL.Operador_Alerta_Variable + " " + oL.Alerta_Variable + oL.Unidad_Variable + "</p>";

                        ActualizarAlertaNotificacion(oL, true, oUsuario.Email_Usuario, oUsuario.Id_Usuario.ToString(), oUsuario.Nombre_Usuario, false);
                        break;
                    case "DESCONECTADO": //Este caso seria cuando el equipo esta desconectado
                        msjs_pedido = msjs_pedido + " no esta reportando eventos, favor de chequear su conexión.</p>";

                        ActualizarAlertaNotificacion(oL, true, oUsuario.Email_Usuario, oUsuario.Id_Usuario.ToString(), oUsuario.Nombre_Usuario, true);
                        break;
                    case "RECONECTADO": //Este caso seria cuando el equipo estaba desconectado y se volvio a conectar
                        msjs_pedido = msjs_pedido + " esta reportando eventos nuevamente.</p>";

                        ActualizarAlertaNotificacion(oL, false, oUsuario.Email_Usuario, oUsuario.Id_Usuario.ToString(), oUsuario.Nombre_Usuario, true);
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
        
        private void ActualizarAlertaNotificacion(Modelo.Lectura oL, bool pEstado, string email, string id_usuario, string nombre_usuario, bool pSin_Conexion_Equipo)
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
            }
            if (pSin_Conexion_Equipo)//Actualizo el EQUIPO el campo SIN_CONEXION_EQUIPO
            {//DESCONECTADO o RECONECTADO
                //actualizo SIN_CONEXION_EQUIPO en la BD para no seguir notificando
                Modelo.Equipo pEquipo = new Modelo.Equipo();
                pEquipo.Sin_Conexion_Equipo = pEstado;
                pEquipo.Id_Equipo = oL.Id_Equipo;
                resultado = Controlador.EquiposDAL.ModificarSinConexion(pEquipo);
                pNotificacion.Id_Variable = "";
                pNotificacion.Nombre_Variable = "";
                pNotificacion.Valor_Variable = "";
                pNotificacion.Operador_Variable = "";
                pNotificacion.Alerta_Notificada = "";
                if (pEstado)
                    pNotificacion.Valor_Leido = "EQUIPO DESCONECTADO.";
                else
                    pNotificacion.Valor_Leido = "EQUIPO RECONECTADO.";
            }
            pNotificacion.Email_Notificacion = email;
            pNotificacion.Id_Conexion = oL.Id_Conexion.ToString();
            pNotificacion.Nombre_Conexion = oL.Nombre_Conexion;
            pNotificacion.Id_Usuario = id_usuario;
            pNotificacion.Nombre_Usuario = nombre_usuario;
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
        }

        private void ChequearDatos()
        {
            try
            {
                string Id_Equipo_Vuelta_Anterior = "";
                foreach (Modelo.Lectura oLectura in Controlador.LecturasDAL.RecuperarUltimasLecturas())
                {
                    decimal dcmValorLectura = Convert.ToDecimal(oLectura.Valor_Lectura.Replace(".", ","));
                    decimal dcmAlertaVariable = 0;
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
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                            case ">=":
                                if (dcmValorLectura >= dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                            case "<":
                                if (dcmValorLectura < dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
                                }
                                break;
                            case "<=":
                                if (dcmValorLectura <= dcmAlertaVariable)
                                {
                                    if (!oLectura.Alerta_Notificada)
                                        EnvioEmailNotificacionAlerta(oLectura, "CONECTADO");
                                }
                                else
                                {
                                    if (oLectura.Alerta_Notificada)
                                        ActualizarAlertaNotificacion(oLectura, false, "", "", "", false);
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
                    //Aca chequeo si paso mucho tiempo desde la ultima lectura, si eso sucede tengo que notificar, el equipo puede tener un problema.
                    //Ojo que lo deberia hacer solo una vez, y no siempre.
                    //Aparte tengo que avisar por el equipo y no por las variables que tiene asociada
                    int Result;
                    bool pEstado = false;
                    Modelo.Equipo pEquipo = new Modelo.Equipo();
                    pEquipo.Id_Equipo = oLectura.Id_Equipo;
                    TimeSpan Diferencia_Entre_Fechas = DateTime.Now.Subtract(Convert.ToDateTime(oLectura.Fecha_Lectura));
                    if (Diferencia_Entre_Fechas.Days > 0)
                        pEstado = true;
                    else
                    {
                        if (Diferencia_Entre_Fechas.Hours > 0)
                            pEstado = true;
                        else
                        {
                            if (Diferencia_Entre_Fechas.Minutes > 5)
                                pEstado = true;
                            else
                                pEstado = false;
                        }
                    }
                    if (!oLectura.Sin_Conexion_Equipo) //Estaba funcionando el equipo y perdio la conexion
                    {
                        if (pEstado) //Si es TRUE significa que hay una falla y la debo enviar al usuario por email
                        {
                            if (Id_Equipo_Vuelta_Anterior != oLectura.Id_Equipo)
                            {
                                pEquipo.Sin_Conexion_Equipo = pEstado;
                                Result = Controlador.EquiposDAL.ModificarSinConexion(pEquipo);
                                EnvioEmailNotificacionAlerta(oLectura, "DESCONECTADO");
                                oLog.LogToFile("Id_Equipo_Vuelta_Anterior: " + Id_Equipo_Vuelta_Anterior + " != " + " oLectura.Id_Equipo: " + oLectura.Id_Equipo);
                                Id_Equipo_Vuelta_Anterior = oLectura.Id_Equipo;
                                oLog.LogToFile("Si es TRUE significa que hay una falla y la debo enviar al usuario por email");
                                oLog.LogToFile("EnvioEmailNotificacionAlerta(oLectura, DESCONECTADO);");
                                oLog.LogToFile("pEstado: " + pEstado.ToString());
                                oLog.LogToFile("Id_Equipo_Vuelta_Anterior: " + Id_Equipo_Vuelta_Anterior +" != "+ " oLectura.Id_Equipo: " + oLectura.Id_Equipo);
                            }
                        }
                    }
                    else //Estaba con falla el equipo, chequemos si ya se soluciono o persiste
                    {
                        if (!pEstado) //Si es FALSE significa que volvio a funcionar
                        {
                            if (Id_Equipo_Vuelta_Anterior != oLectura.Id_Equipo) //Si son iguales significa que entro en la vuelta anterior.
                            {
                                pEquipo.Sin_Conexion_Equipo = pEstado;
                                Result = Controlador.EquiposDAL.ModificarSinConexion(pEquipo);
                                EnvioEmailNotificacionAlerta(oLectura, "RECONECTADO");
                                oLog.LogToFile("Id_Equipo_Vuelta_Anterior: " + Id_Equipo_Vuelta_Anterior + " != " + " oLectura.Id_Equipo: " + oLectura.Id_Equipo);
                                Id_Equipo_Vuelta_Anterior = oLectura.Id_Equipo;
                                oLog.LogToFile("Si es FALSE significa que volvio a funcionar");
                                oLog.LogToFile("EnvioEmailNotificacionAlerta(oLectura, RECONECTADO);");
                                oLog.LogToFile("pEstado: " + pEstado.ToString());
                                oLog.LogToFile("Id_Equipo_Vuelta_Anterior: " + Id_Equipo_Vuelta_Anterior + " != " + " oLectura.Id_Equipo: " + oLectura.Id_Equipo);
                            }
                        }
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
