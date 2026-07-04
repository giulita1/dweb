using desarrolloweb.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Reserva
    {
        
        public int Id_Reserva { get; set; }
        public Habitacion Hab { get; set; }
        public Usuario Usuario_ { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Huespedes { get; set; }
        public bool IncluyeDesayuno { get; set; }
        public double Total { get; set; }
        public string Estado { get; set; }
        public DateTime FechaReserva { get; set; }

    }
}
