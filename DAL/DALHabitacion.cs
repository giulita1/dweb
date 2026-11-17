using BE;
using SEG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace DAL
{
    public class DALHabitacion : DALconexion
    {
        public Habitacion ObtenerPorId(int id)
        {
            Habitacion hab = null;

            string query = @"
                SELECT Id_Habitacion, Nombre, Tipo, Descripcion, 
                       PrecioPorNoche, Huespedes, ImagenUrl
                FROM Habitacion
                WHERE Id_Habitacion = @Id";

            Conectar();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hab = new Habitacion
                        {
                            Id_Habitacion = (int)reader["Id_Habitacion"],
                            Nombre = reader["Nombre"].ToString(),
                            Tipo = reader["Tipo"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            PrecioPorNoche = Convert.ToDouble(reader["PrecioPorNoche"]),
                            Huespedes = (int)reader["Huespedes"],
                            ImagenUrl = reader["ImagenUrl"].ToString()
                        };
                    }
                }
                Desconectar();
            }

            return hab;
        }

        public List<Habitacion> ObtenerHabitacionesDisponibles(DateTime inicio, DateTime fin, int huespedes)
        {
            List<Habitacion> candidatas = new List<Habitacion>();

            // 1. Traemos las habitaciones que cumplen con la capacidad de huéspedes
            string queryHab = @"
                SELECT Id_Habitacion, Nombre, Tipo, Descripcion, 
                       PrecioPorNoche, Huespedes, ImagenUrl
                FROM Habitacion
                WHERE Huespedes >= @Huespedes
                ORDER BY PrecioPorNoche ASC";

            SqlParameter[] pHab = { new SqlParameter("@Huespedes", huespedes) };
            DataTable dtHab = LeerText(queryHab, pHab);

            foreach (DataRow dr in dtHab.Rows)
            {
                candidatas.Add(new Habitacion
                {
                    Id_Habitacion = Convert.ToInt32(dr["Id_Habitacion"]),
                    Nombre = dr["Nombre"].ToString(),
                    Tipo = dr["Tipo"].ToString(),
                    Descripcion = dr["Descripcion"].ToString(),
                    PrecioPorNoche = Convert.ToDouble(dr["PrecioPorNoche"]),
                    Huespedes = Convert.ToInt32(dr["Huespedes"]),
                    ImagenUrl = dr["ImagenUrl"].ToString()
                });
            }

            // 2. Traemos las reservas activas para desencriptar sus fechas y verificar solapamientos
            string queryRes = @"
                SELECT Id_Habitacion, FechaLlegada, FechaSalida
                FROM Reservas
                WHERE Estado != 'Cancelada'";

            DataTable dtRes = LeerText(queryRes);
            HashSet<int> habitacionesOcupadas = new HashSet<int>();

            foreach (DataRow dr in dtRes.Rows)
            {
                string llegadaStr = encriptar.DesencriptarAES(dr["FechaLlegada"].ToString());
                string salidaStr = encriptar.DesencriptarAES(dr["FechaSalida"].ToString());

                DateTime fechaLlegada = DateTime.ParseExact(llegadaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaSalida = DateTime.ParseExact(salidaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                // Si las fechas se solapan, marcamos la habitación como ocupada
                if (fechaLlegada < fin && fechaSalida > inicio)
                {
                    habitacionesOcupadas.Add(Convert.ToInt32(dr["Id_Habitacion"]));
                }
            }

            // 3. Devolvemos solo las habitaciones que no están ocupadas en ese rango
            return candidatas.Where(h => !habitacionesOcupadas.Contains(h.Id_Habitacion)).ToList();
        }

        public List<Habitacion> ObtenerTodasParaDVV()
        {
            List<Habitacion> lista = new List<Habitacion>();

            string query = @"SELECT Id_Habitacion, Nombre, Tipo, Descripcion, 
                                     PrecioPorNoche, Huespedes, ImagenUrl, DVH
                             FROM Habitacion";

            DataTable dt = LeerText(query);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new Habitacion
                {
                    Id_Habitacion = Convert.ToInt32(dr["Id_Habitacion"]),
                    Nombre = dr["Nombre"].ToString(),
                    Tipo = dr["Tipo"].ToString(),
                    Descripcion = dr["Descripcion"].ToString(),
                    PrecioPorNoche = Convert.ToDouble(dr["PrecioPorNoche"]),
                    Huespedes = Convert.ToInt32(dr["Huespedes"]),
                    ImagenUrl = dr["ImagenUrl"].ToString(),
                    DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0
                });
            }

            return lista;
        }

        public void ActualizarDVH(int idHabitacion, int dvh)
        {
            string query = "UPDATE Habitacion SET DVH = @dvh WHERE Id_Habitacion = @id";
            SqlParameter[] p = {
                new SqlParameter("@dvh", dvh),
                new SqlParameter("@id",  idHabitacion)
            };
            EscribirText(query, p);
        }
    }
}