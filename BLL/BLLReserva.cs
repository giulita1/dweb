using BE;
using desarrolloweb.BLL;
using SEG.singleton;
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
        BLLbitacora bllBitacora = new BLLbitacora();
        BLLDVV bllDvv = new BLLDVV();

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
            int idGenerado = dal.CrearReserva(reserva);
            SincronizarDigitos(dal.ObtenerReservaPorId(idGenerado), dal);
            int idUsuarioBitacora = SingletonSession.Instancia.Usuario.Id_Usuario;

            bllBitacora.InsertarBitacora(idUsuarioBitacora, "Reserva creada: " + reserva.Id_Reserva, "Reservas", "3");

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
            SincronizarDigitos(dal.ObtenerReservaPorId(idReserva), dal);
            int idUsuarioBitacora = SingletonSession.Instancia.Usuario.Id_Usuario;

            bllBitacora.InsertarBitacora(idUsuarioBitacora, "Reserva cancelada: " + idReserva, "Reservas", "4");

        }
        private void SincronizarDigitos(Reserva reservaBD, DalReserva dal)
        {
            if (reservaBD != null)
            {
                SEG.DigitoVerificador motorDV = new SEG.DigitoVerificador();
                int nuevoDvh = motorDV.CalcularDVH(reservaBD);

                dal.ActualizarDVH(reservaBD.Id_Reserva, nuevoDvh);
                bllDvv.RecalcularDVV("Reservas");
            }
        }
        public List<Reserva> ObtenerTodasParaDVV()
        {
            DalReserva dal = new DalReserva();
            return dal.ObtenerTodasParaDVV();
        }
    }
}
