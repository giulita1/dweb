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

        public string ObtenerUltimaOperacion(string tabla, int idRegistro)
        {
            try
            {
                string sql = idRegistro == -1
                    ? @"SELECT TOP 1 Operacion, Fecha FROM AuditoriaTablas 
                WHERE Tabla = @tabla ORDER BY Fecha DESC"
                    : @"SELECT TOP 1 Operacion, Fecha FROM AuditoriaTablas 
                WHERE Tabla = @tabla AND Id_Registro = @id ORDER BY Fecha DESC";

                List<SqlParameter> parametros = new List<SqlParameter>
        {
            new SqlParameter("@tabla", tabla)
        };

                if (idRegistro != -1)
                    parametros.Add(new SqlParameter("@id", idRegistro));

                DataTable dt = accesos.LeerText(sql, parametros.ToArray());

                if (dt != null && dt.Rows.Count > 0)
                {
                    string operacion = dt.Rows[0]["Operacion"].ToString();
                    string fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"])
                                              .ToString("dd/MM/yyyy HH:mm");
                    return $"{operacion} el {fecha}";
                }

                return "sin registro de auditoría";
            }
            catch
            {
                return "no se pudo determinar";
            }
        }
    }
}
