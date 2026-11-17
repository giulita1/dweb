using BE;
using SEG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DAL
{
    public class DALreserva
    {
        public class DalReserva : DALconexion
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
                    r.Hab = new Habitacion();
                    r.Usuario_ = new desarrolloweb.BE.Usuario();
                    r.Id_Reserva = Convert.ToInt32(dr["Id_Reserva"]);
                    r.Hab.Id_Habitacion = Convert.ToInt32(dr["Id_Habitacion"]);
                    r.Usuario_.Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]);
                    r.Estado = dr["Estado"].ToString();

                    string llegadaStr = encriptar.DesencriptarAES(dr["FechaLlegada"].ToString());
                    string salidaStr = encriptar.DesencriptarAES(dr["FechaSalida"].ToString());
                    string totalStr = encriptar.DesencriptarAES(dr["Total"].ToString());

                    r.FechaLlegada = DateTime.ParseExact(llegadaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    r.FechaSalida = DateTime.ParseExact(salidaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    r.Total = double.Parse(totalStr, CultureInfo.InvariantCulture);

                    r.Huespedes = Convert.ToInt32(dr["Huespedes"]);
                    r.IncluyeDesayuno = Convert.ToBoolean(dr["IncluyeDesayuno"]);
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

            public int CrearReserva(Reserva reserva)
            {
                string query = @"
                INSERT INTO Reservas 
                    (Id_Habitacion, Id_Usuario, FechaLlegada, FechaSalida, 
                     Huespedes, IncluyeDesayuno, Total, Estado, FechaReserva)
                VALUES 
                    (@HabitacionId, @UsuarioId, @FechaLlegada, @FechaSalida,
                     @Huespedes, @IncluyeDesayuno, @Total, @Estado, @FechaReserva); SELECT SCOPE_IDENTITY();";

                string llegadaCifrada = encriptar.EncriptarAES(reserva.FechaLlegada.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                string salidaCifrada = encriptar.EncriptarAES(reserva.FechaSalida.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                string totalCifrado = encriptar.EncriptarAES(reserva.Total.ToString(CultureInfo.InvariantCulture));

                SqlParameter[] p = {
                    new SqlParameter("@HabitacionId", reserva.Hab.Id_Habitacion),
                    new SqlParameter("@UsuarioId", reserva.Usuario_.Id_Usuario),
                    new SqlParameter("@FechaLlegada", llegadaCifrada),
                    new SqlParameter("@FechaSalida", salidaCifrada),
                    new SqlParameter("@Huespedes", reserva.Huespedes),
                    new SqlParameter("@IncluyeDesayuno", reserva.IncluyeDesayuno),
                    new SqlParameter("@Total", totalCifrado),
                    new SqlParameter("@Estado", reserva.Estado),
                    new SqlParameter("@FechaReserva", DateTime.Now)
                };

                return EscribirYDevolverId_62_RS(query, p);
            }

            public Reserva ObtenerReservaPorId(int idReserva)
            {
                string query = @"SELECT * FROM Reservas WHERE Id_Reserva = @IdReserva";
                SqlParameter[] p = { new SqlParameter("@IdReserva", idReserva) };

                DataTable dt = LeerText(query, p);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return MapearReserva(dt.Rows[0]);
                }
                return null;
            }

            public List<Reserva> ObtenerTodasParaDVV()
            {
                List<Reserva> reservas = new List<Reserva>();
                string query = @"SELECT * FROM Reservas";
                DataTable dt = LeerText(query);

                foreach (DataRow dr in dt.Rows)
                {
                    reservas.Add(MapearReserva(dr));
                }
                return reservas;
            }

            private Reserva MapearReserva(DataRow dr)
            {
                Reserva r = new Reserva();
                r.Hab = new Habitacion();
                r.Usuario_ = new desarrolloweb.BE.Usuario();

                r.Id_Reserva = Convert.ToInt32(dr["Id_Reserva"]);
                r.Hab.Id_Habitacion = Convert.ToInt32(dr["Id_Habitacion"]);
                r.Usuario_.Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]);
                r.Estado = dr["Estado"].ToString();

                string llegadaStr = encriptar.DesencriptarAES(dr["FechaLlegada"].ToString());
                string salidaStr = encriptar.DesencriptarAES(dr["FechaSalida"].ToString());
                string totalStr = encriptar.DesencriptarAES(dr["Total"].ToString());

                r.FechaLlegada = DateTime.ParseExact(llegadaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                r.FechaSalida = DateTime.ParseExact(salidaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                r.Total = double.Parse(totalStr, CultureInfo.InvariantCulture);

                r.Huespedes = Convert.ToInt32(dr["Huespedes"]);
                r.IncluyeDesayuno = Convert.ToBoolean(dr["IncluyeDesayuno"]);
                r.FechaReserva = Convert.ToDateTime(dr["FechaReserva"]);
                r.DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0;

                return r;
            }

            public void ActualizarDVH(int idReserva, int dvh)
            {
                string sql = "UPDATE Reservas SET DVH = @DVH WHERE Id_Reserva = @id";
                SqlParameter[] p = {
                    new SqlParameter("@DVH", dvh),
                    new SqlParameter("@id", idReserva)
                };
                EscribirText(sql, p);
            }
        }
    }
}