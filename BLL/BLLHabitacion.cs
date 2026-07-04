using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLLHabitacion
    {
        public Habitacion ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID de habitación inválido.");

            DALHabitacion dal = new DALHabitacion();
            Habitacion hab = dal.ObtenerPorId(id);

            if (hab == null)
                throw new Exception("No se encontró la habitación.");

            return hab;
        }
        public List<Habitacion> ObtenerHabitacionesDisponibles(DateTime inicio, DateTime fin, int huespedes)
        {
            if (inicio >= fin)
                throw new ArgumentException("La fecha de salida debe ser posterior a la de llegada.");

            if (inicio < DateTime.Today)
                throw new ArgumentException("La fecha de llegada no puede ser en el pasado.");

            if (huespedes < 1 || huespedes > 10)
                throw new ArgumentException("La cantidad de huéspedes debe estar entre 1 y 10.");

            DALHabitacion dal = new DALHabitacion();
            return dal.ObtenerHabitacionesDisponibles(inicio, fin, huespedes);
        }
    }
}
