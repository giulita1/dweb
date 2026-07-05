using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALreserva
    {
        public class DalReserva:DALconexion
        {
            public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
            {
                List<Reserva> reservas = new List<Reserva>();

                string query = @"
                    SELECT r.Id_Reserva, r.Id_Habitacion, r.Id_Usuario, r.Estado,
                           r.FechaLlegada, r.FechaSalida, r.Huespedes,
                           r.IncluyeDesayuno, r.Total, r.FechaReserva,
                           h.Nombre, h.Tipo, h.ImagenUrl
                    FROM Reservas r
                    INNER JOIN Habitacion h ON r.Id_Habitacion = h.Id_Habitacion
                    WHERE r.Id_Usuario = @IdUsuario
                    ORDER BY r.FechaReserva DESC";

                SqlParameter[] p = { new SqlParameter("@IdUsuario", idUsuario) };
                DataTable dt = LeerText(query, p);

                foreach (DataRow dr in dt.Rows)
                {
                    Reserva r = new Reserva();
                    r.Id_Reserva = Convert.ToInt32(dr["Id_Reserva"]);
                    r.Hab.Id_Habitacion = Convert.ToInt32(dr["Id_Habitacion"]);
                    r.Usuario_.Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]);
                    r.Estado = dr["Estado"].ToString();
                    r.FechaLlegada = Convert.ToDateTime(dr["FechaLlegada"]);
                    r.FechaSalida = Convert.ToDateTime(dr["FechaSalida"]);
                    r.Huespedes = Convert.ToInt32(dr["Huespedes"]);
                    r.IncluyeDesayuno = Convert.ToBoolean(dr["IncluyeDesayuno"]);
                    r.Total = Convert.ToDouble(dr["Total"]);
                    r.FechaReserva = Convert.ToDateTime(dr["FechaReserva"]);
                    r.Hab.Nombre = dr["Nombre"].ToString();
                    r.Hab.Tipo = dr["Tipo"].ToString();
                    r.Hab.ImagenUrl = dr["ImagenUrl"].ToString();

                    reservas.Add(r);
                }

                return reservas;
            }

            public void CancelarReserva(int idReserva, int idUsuario)
            {
                string query = @"UPDATE Reservas 
                     SET Estado = 'Cancelada' 
                     WHERE Id_Reserva = @IdReserva 
                     AND Id_Usuario = @IdUsuario
                     AND Estado != 'Cancelada'";

                SqlParameter[] p = {
        new SqlParameter("@IdReserva", idReserva),
        new SqlParameter("@IdUsuario", idUsuario)
    };

                EscribirText(query, p);
            }
            public void CrearReserva(Reserva reserva)
            {
                string query = @"
                INSERT INTO Reservas 
                    (Id_Habitacion, Id_Usuario, FechaLlegada, FechaSalida, 
                     Huespedes, IncluyeDesayuno, Total, Estado, FechaReserva)
                VALUES 
                    (@HabitacionId, @UsuarioId, @FechaLlegada, @FechaSalida,
                     @Huespedes, @IncluyeDesayuno, @Total, @Estado, @FechaReserva)";

                Conectar();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@HabitacionId", reserva.Hab.Id_Habitacion);
                    cmd.Parameters.AddWithValue("@UsuarioId", reserva.Usuario_.Id_Usuario);
                    cmd.Parameters.AddWithValue("@FechaLlegada", reserva.FechaLlegada);
                    cmd.Parameters.AddWithValue("@FechaSalida", reserva.FechaSalida);
                    cmd.Parameters.AddWithValue("@Huespedes", reserva.Huespedes);
                    cmd.Parameters.AddWithValue("@IncluyeDesayuno", reserva.IncluyeDesayuno);
                    cmd.Parameters.AddWithValue("@Total", reserva.Total);
                    cmd.Parameters.AddWithValue("@Estado", reserva.Estado);
                    cmd.Parameters.AddWithValue("@FechaReserva", DateTime.Now);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
