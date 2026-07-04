using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.DALreserva;

namespace BLL
{
    public class BLLReserva
    {
            public void CrearReserva(Reserva reserva)
            {
                if (reserva.FechaLlegada >= reserva.FechaSalida)
                    throw new ArgumentException("Las fechas de la reserva son inválidas.");

                if (reserva.Hab.Id_Habitacion <= 0)
                    throw new ArgumentException("Habitación inválida.");

                if (reserva.Usuario_.Id_Usuario <= 0)
                    throw new ArgumentException("Usuario inválido.");

                if (reserva.Total <= 0)
                    throw new ArgumentException("El total de la reserva es inválido.");

                DalReserva dal = new DalReserva();
                dal.CrearReserva(reserva);
            }

        public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Usuario inválido.");

            DalReserva dal = new DalReserva();
            return dal.ObtenerReservasPorUsuario(idUsuario);
        }

        public void CancelarReserva(int idReserva, int idUsuario)
        {
            if (idReserva <= 0)
                throw new ArgumentException("Reserva inválida.");

            if (idUsuario <= 0)
                throw new ArgumentException("Usuario inválido.");

            DalReserva dal = new DalReserva();
            dal.CancelarReserva(idReserva, idUsuario);
        }

    }
}
