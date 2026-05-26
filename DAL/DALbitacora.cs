using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace desarrolloweb.DAL
{
    public class DALbitacora : DALconexion
    {
        public int insertarbitacora(BE.Bitacora BT)
        {
            string query = @"INSERT INTO Bitacora (Id_Usuario, Actividad, Criticidad, Fecha, Hora, DVH) 
                             VALUES (@Id_Usuario, @Actividad, @Criticidad, @Fecha, @Hora, @DVH)";
            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@Id_Usuario", BT.Id_Usuario);
            sqlParameter[1] = new SqlParameter("@Actividad", BT.Actividad);
            sqlParameter[2] = new SqlParameter("@Criticidad", BT.Criticidad);
            sqlParameter[3] = new SqlParameter("@Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
            sqlParameter[4] = new SqlParameter("@Hora", DateTime.Now.ToString("HH:mm:ss"));
            sqlParameter[5] = new SqlParameter("@DVH", (object)BT.DVH ?? DBNull.Value);
            return EscribirText(query, sqlParameter);
        }
        public DataTable ListarBitacora()
        {
            try
            {
                string query = @"SELECT b.Id_Bitacora, b.Id_Usuario, b.Fecha, b.Hora, b.Actividad, b.Criticidad, u.Nombre, u.Apellido 
                                 FROM Bitacora b  
                                 INNER JOIN Usuarios u ON b.Id_Usuario = u.Id_Usuario 
                                 WHERE CONVERT(date, b.Fecha, 103) >= CAST(GETDATE() - 3 AS DATE)";

                return LeerText(query);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error técnico en la base de datos al intentar listar la bitácora. Detalle: " + ex.Message);
            }
        }

        public DataTable FiltrarBitacora(string login, string modulo, string evento, string criticidad, DateTime desde, DateTime hasta)
        {
            try
            {
                DateTime fechaDesde = desde.Date;
                DateTime fechaHasta = hasta.Date;
                string query = @"SELECT b.Id_Bitacora, b.Id_Usuario, b.Fecha, b.Hora, b.Actividad, b.Criticidad, u.Nombre, u.Apellido 
                                 FROM Bitacora b 
                                 INNER JOIN Usuarios u ON b.Id_Usuario = u.Id_Usuario 
                                 WHERE CONVERT(date, b.Fecha, 103) >= @Desde 
                                 AND CONVERT(date, b.Fecha, 103) <= @Hasta";

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter("@Desde", fechaDesde),
                    new SqlParameter("@Hasta", fechaHasta)
                };

                if (login != "Todos" && !string.IsNullOrEmpty(login))
                {
                    query += " AND u.usuario = @Login";
                    parametros.Add(new SqlParameter("@Login", login));
                }
                if (evento != "Todos" && !string.IsNullOrEmpty(evento))
                {
                    query += " AND b.Actividad = @Evento";
                    parametros.Add(new SqlParameter("@Evento", evento));
                }
                if (criticidad != "Todos" && !string.IsNullOrEmpty(criticidad))
                {
                    query += " AND b.Criticidad = @Criticidad";
                    parametros.Add(new SqlParameter("@Criticidad", criticidad));
                }

                return LeerText(query, parametros.ToArray());
            }
            catch (SqlException ex)
            {
                throw new Exception("Error técnico en la base de datos al filtrar la bitácora. Detalle: " + ex.Message);
            }
       }
    }
}
