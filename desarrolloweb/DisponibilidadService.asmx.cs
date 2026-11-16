using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace desarrolloweb
{
    /// <summary>
    /// Descripción breve de DisponibilidadService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class DisponibilidadService : System.Web.Services.WebService
    {
        [WebMethod(Description = "Consulta habitaciones disponibles para las fechas y cantidad de huéspedes indicados.")]
        public List<HabitacionDTO> ConsultarDisponibilidad(string fechaLlegada, string fechaSalida, int huespedes)
        {
            try
            {
                if (!DateTime.TryParse(fechaLlegada, out DateTime llegada))
                    throw new Exception("Fecha de llegada inválida. Formato esperado: yyyy-MM-dd");

                if (!DateTime.TryParse(fechaSalida, out DateTime salida))
                    throw new Exception("Fecha de salida inválida. Formato esperado: yyyy-MM-dd");

                BLLHabitacion bll = new BLLHabitacion();
                List<Habitacion> habitaciones = bll.ObtenerHabitacionesDisponibles(llegada, salida, huespedes);

                List<HabitacionDTO> resultado = new List<HabitacionDTO>();
                foreach (Habitacion h in habitaciones)
                {
                    resultado.Add(new HabitacionDTO
                    {
                        Id_Habitacion = h.Id_Habitacion,
                        Nombre = h.Nombre,
                        Tipo = h.Tipo,
                        Descripcion = h.Descripcion,
                        PrecioPorNoche = h.PrecioPorNoche,
                        Capacidad = h.Huespedes,
                        ImagenUrl = h.ImagenUrl
                    });
                }

                return resultado;
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Error de validación: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar disponibilidad: " + ex.Message);
            }
        }
    }

    public class HabitacionDTO
    {
        public int Id_Habitacion { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public double PrecioPorNoche { get; set; }
        public int Capacidad { get; set; }
        public string ImagenUrl { get; set; }
    }
}

