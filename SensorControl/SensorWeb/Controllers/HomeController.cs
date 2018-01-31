using Controlador;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SensorWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grilla()
        {
            return View();
        }

        public ActionResult GraficoExtremos()
        {
            return View();
        }

        public ActionResult GraficoNotificaciones()
        {
            return View();
        }

        [HttpPost]
        public ContentResult GetChart(string id)
        {
            var lecturas = LecturasDAL
                .RecuperarLecturaGrafico(id)
                ;

            var labels = lecturas
                .Select(x => x.Fecha_Lectura)
                .Distinct()
                .OrderBy(x => x)
                .Where((_, i) => i % 2 == 0)
                ;

            var tmp = lecturas.FirstOrDefault();
            var title = "Grafico en tiempo real";
            var description = tmp.Nombre_Equipo + " - " + tmp.Nombre_Conexion + " - " + tmp.Nombre_Ubicacion;

            var chart = new
            {
                type = "bar",
                options = new
                {
                    xAxes = new[]{
                        new {
                            type="time",
                            distribution = "linear",
                            ticks = new {
                                source = "labels"
                            }
                        }
                    }
                },
                data = new
                {
                    labels = labels,
                    datasets = lecturas
                        .GroupBy(x => x.Id_Variable)
                        .Select(x => {
                            var t = x.First();
                            return new
                            {
                                label = String.Format(" {0}", t.Nombre_Variable),
                                type = t.Id_Variable == "1" ? "line" : "bar",
                                data = x.OrderBy(m => m.Fecha_Lectura)
                                        .Select(y => new {
                                            t = y.Fecha_Lectura,
                                            y = y.Valor_Lectura
                                        }),

                            };
                        })
                }
            };

            return Content(JsonConvert.SerializeObject(new { title, description, chart }), "application/json");
        }

        public ContentResult GetChartMinimaMaxima(string id)
        {
            DateTime f_Hasta = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " 00:00:00");
            DateTime f_Desde = f_Hasta.AddDays(-10);
            var lecturas = LecturasDAL
                .ValoresMinimos_y_Maximos(id, f_Desde, f_Hasta)
                ;

            var labels = lecturas
                .Select(x => x.Fecha_Lectura)
                .Distinct()
                .OrderBy(x => x)
                .Where((_, i) => i % 2 == 0)
                ;

            var tmp = lecturas.FirstOrDefault();
            var title = " Valores Minimos y Maximos";
            var description = tmp.Nombre_Equipo + " - " + tmp.Nombre_Conexion + " - " + tmp.Nombre_Ubicacion;

            var chart = new
            {
                type = "bar",
                options = new
                {
                    xAxes = new[]{
                        new {
                            type="time",
                            distribution = "linear",
                            ticks = new {
                                source = "labels"
                            }
                        }
                    }
                },
                data = new
                {
                    labels = labels,
                    datasets = lecturas
                        .GroupBy(x => x.Id_Variable)
                        .Select(x => {
                            var t = x.First();
                            return new
                            {
                                label = String.Format(" {0}", t.Nombre_Variable),
                                type = t.Id_Variable == "2" ? "line" : "bar",
                                data = x.OrderBy(m => m.Fecha_Lectura)
                                        .Select(y => new {
                                            t = y.Fecha_Lectura,
                                            y = y.Valor_Minimo
                                        }),

                            };
                        })
                }
            };

            return Content(JsonConvert.SerializeObject(new { title, description, chart }), "application/json");
        }

    }
}