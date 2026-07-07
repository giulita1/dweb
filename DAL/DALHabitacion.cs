using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALHabitacion:DALconexion
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
            List<Habitacion> habitaciones = new List<Habitacion>();

            string query = @"
                            SELECT h.Id_Habitacion, h.Nombre, h.Tipo, h.Descripcion, 
                                   h.PrecioPorNoche, h.Huespedes, h.ImagenUrl
                            FROM Habitacion h
                            WHERE h.Huespedes >= @Huespedes
                              AND h.Id_Habitacion NOT IN (
                                  SELECT r.Id_Habitacion
                                  FROM Reservas r
                                  WHERE r.Estado != 'Cancelada'
                                    AND r.FechaLlegada < @Fin
                                    AND r.FechaSalida  > @Inicio
                              )
                            ORDER BY h.PrecioPorNoche ASC";
            SqlCommand cmd = new SqlCommand(query, con);
            
                cmd.Parameters.AddWithValue("@Huespedes", huespedes);
                cmd.Parameters.AddWithValue("@Inicio", inicio);
                cmd.Parameters.AddWithValue("@Fin", fin);

            Conectar();
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                Habitacion hab = new Habitacion();
                hab.Id_Habitacion = Convert.ToInt32(reader["Id_Habitacion"]);
                hab.Nombre = reader["Nombre"].ToString();
                hab.Tipo = reader["Tipo"].ToString();
                hab.Descripcion = reader["Descripcion"].ToString();
                hab.PrecioPorNoche = Convert.ToDouble(reader["PrecioPorNoche"]);
                hab.Huespedes = Convert.ToInt32(reader["Huespedes"]);
                hab.ImagenUrl = reader["ImagenUrl"].ToString();
                habitaciones.Add(hab);
            
            }
            reader.Close();
            Desconectar();
            return habitaciones;
        }
    }
}
