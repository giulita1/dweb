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
    public class DVV
    {
        private DALconexion accesos = new DALconexion();

        public List<int> ObtenerTodosLosDVH(string nombreTabla)
        {
            List<int> listaDVH = new List<int>();
            try
            {

                string sql = $"SELECT DVH FROM [{nombreTabla}] WHERE DVH IS NOT NULL";

                DataTable dt = accesos.LeerText(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    listaDVH.Add(Convert.ToInt32(dr["Dvh"]));
                }

                return listaDVH;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener DVHs de la tabla {nombreTabla}: {ex.Message}");
            }
        }
        public void ActualizarSumaDVV(string nombreTabla, int nuevoDvv)
        {
            try
            {
                string sql = "UPDATE DVV SET ValorDvv = @dvv WHERE NombreTabla = @tabla";
                SqlParameter[] p = {
            new SqlParameter("@dvv", nuevoDvv),
            new SqlParameter("@tabla", nombreTabla)
        };

                if (accesos.EscribirText(sql, p) == 0)
                {
                    string sqlInsert = "INSERT INTO DVV (NombreTabla, ValorDvv) VALUES (@tabla, @dvv)";
                    SqlParameter[] pInsert = {
                new SqlParameter("@dvv", nuevoDvv),
                new SqlParameter("@tabla", nombreTabla)
            };
                    accesos.EscribirText(sqlInsert, pInsert);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la tabla DVV: " + ex.Message);
            }
        }

        public int ObtenerDvvGuardado(string nombreTabla)
        {
            try
            {
                string sql = "SELECT ValorDvv FROM DVV WHERE NombreTabla = @tabla";
                SqlParameter[] p = { new SqlParameter("@tabla", nombreTabla) };

                DataTable dt = accesos.LeerText(sql, p);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["ValorDvv"]);
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener DVV guardado de la tabla {nombreTabla}: {ex.Message}");
            }
        }

        public void LimpiarAuditoria()
        {
            string sql = "DELETE FROM AuditoriaTablas";
            accesos.EscribirText(sql, null);
        }

        public List<Infraccion> ObtenerOperaciones(string tabla)
        {
            List<Infraccion> operaciones = new List<Infraccion>();

            try
            {
                string sql = @"SELECT Operacion, Id_Registro, Fecha
                       FROM AuditoriaTablas
                       WHERE Tabla = @tabla
                       ORDER BY Fecha DESC";


                SqlParameter[] p =
                {
            new SqlParameter("@tabla", tabla)
        };


                DataTable dt = accesos.LeerText(sql, p);



                foreach (DataRow dr in dt.Rows)
                {
                    operaciones.Add(new Infraccion
                    {
                        Tabla = tabla,

                        IdRegistro =
                            dr["Id_Registro"].ToString(),

                        Operacion =
                            $"{dr["Operacion"]} ({Convert.ToDateTime(dr["Fecha"]):dd/MM/yyyy HH:mm})"
                    });
                }


                return operaciones;
            }
            catch (Exception ex)
            {
                return new List<Infraccion>
                {
                    new Infraccion
                    {
                        Tabla = tabla,
                        IdRegistro = "ERROR",
                        Operacion = ex.Message
                    }
                };
            }
        }

    }

}


       
    
