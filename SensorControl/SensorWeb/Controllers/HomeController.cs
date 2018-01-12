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
            var lecturas = LecturasDAL.RecuperarLecturaGrafico();

            var labels = lecturas
                .Select(x => x.Fecha_Lectura)
                .Distinct()
                .OrderBy(x => x)
                .Where((_,i) => i%2 ==0)
                ;



            var chart = new {
                type= "bar",
                options = new {
                    xAxes= new[]{
                        new {
                            type="time",
                            distribution = "linear",
                            ticks = new {
                                source = "labels"
                            }
                        }
                    }
                },
                data= new {
                    labels = labels,
                    datasets = lecturas
                        .GroupBy(x => new { x.Id_Equipo, x.Id_Variable })
                        .Select(x => {
                            var t = x.First();
                            return new
                            {
                                label = String.Format("{0} {1}", t.Nombre_Equipo, t.Nombre_Variable),
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

            ViewBag.Chart = JsonConvert.SerializeObject(chart);

            return View();
        }
    }
}