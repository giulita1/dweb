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
            int FA = 0;

            SqlParameter[] sqlParameter = new SqlParameter[5];
            sqlParameter[0] = new SqlParameter("@Usu", BT.Usu);
            sqlParameter[1] = new SqlParameter("@FechaCambio", BT.FechaCambio);
            sqlParameter[2] = new SqlParameter("@Descripcion", BT.Descripcion);
            sqlParameter[3] = new SqlParameter("@Modulo", BT.Modulo);
            sqlParameter[4] = new SqlParameter("@Criticidad", BT.Criticidad);
            FA = EscribirText("CargarBitacora", sqlParameter);
            return FA;
        }
        public DataTable ListarBitacora()
        {
            try
            {
                string query = " SELECT b.IdBitacora, b.idusuario, CONVERT(varchar(10), b.FechaCambio, 120) AS Fecha, CONVERT(varchar(8), b.FechaCambio, 108) AS Hora, b.Descripcion, b.Modulo, b.Criticidad, u.Nombre, u.Apellido FROM Bitacora b  INNER JOIN Usuarios u ON b.idusuario = u.Id_Usuario WHERE b.FechaCambio >= GETDATE() - 3";
                return LeerText(query);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error técnico en la base de datos al intentar listar usuarios. Detalle: " + ex.Message);
            }
        }

        public DataTable FiltrarBitacora(string login, string modulo, string evento, string criticidad, DateTime desde, DateTime hasta)
        {
            try
            {
                DateTime fechaDesde = desde.Date;
                DateTime fechaHasta = hasta.Date.AddDays(1).AddTicks(-1);
                string query = @"SELECT b.IdBitacora, b.idusuario, CONVERT(varchar(10), b.FechaCambio, 120) AS Fecha,
                                              CONVERT(varchar(8), b.FechaCambio, 108) AS Hora, 
                                              b.Descripcion, b.Modulo, b.Criticidad, 
                                              u.Nombre, u.Apellido 
                                       FROM Bitacora b 
                                       INNER JOIN Usuarios u ON b.idusuario = u.Id_Usuario 
                                       WHERE b.FechaCambio BETWEEN @Desde AND @Hasta";

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter("@Desde", fechaDesde),
                    new SqlParameter("@Hasta", fechaHasta)
                };

                if (login != "Todos" && !string.IsNullOrEmpty(login))
                {
                    query += " AND b.Usu = @Login";
                    parametros.Add(new SqlParameter("@Login", login));
                }

                if (modulo != "Todos" && !string.IsNullOrEmpty(modulo))
                {
                    query += " AND b.Modulo = @Modulo";
                    parametros.Add(new SqlParameter("@Modulo", modulo));
                }

                if (evento != "Todos" && !string.IsNullOrEmpty(evento))
                {
                    query += " AND b.Descripcion = @Evento";
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
