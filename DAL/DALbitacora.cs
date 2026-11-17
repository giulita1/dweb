using DAL;
using SEG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace desarrolloweb.DAL
{
    public class DALbitacora : DALconexion
    {
        public int insertarbitacora(BE.Bitacora BT)
        {
            try
            {
                string query = @"INSERT INTO Bitacora (Id_Usuario, Actividad, Modulo, Criticidad, Fecha, Hora) 
                                 VALUES (@Id_Usuario, @Actividad, @modulo, @Criticidad, @Fecha, @Hora);
                                 SELECT SCOPE_IDENTITY();";
                SqlParameter[] sqlParameter = new SqlParameter[6];
                sqlParameter[0] = new SqlParameter("@Id_Usuario", BT.Id_Usuario);
                sqlParameter[1] = new SqlParameter("@Actividad", BT.Actividad);
                sqlParameter[2] = new SqlParameter("@Modulo", BT.modulo);
                sqlParameter[3] = new SqlParameter("@Criticidad", BT.Criticidad);
                sqlParameter[4] = new SqlParameter("@Fecha", BT.Fecha);
                sqlParameter[5] = new SqlParameter("@Hora", BT.Hora);
                return EscribirYDevolverId_62_RS(query, sqlParameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en DAL al insertar Bitacora: " + ex.Message);
            }
        }

        public DataTable ListarBitacora()
        {
            try
            {
                string query = @"SELECT b.Id_Bitacora, b.Id_Usuario, b.Fecha, b.Hora, b.Actividad, b.Modulo, b.Criticidad, u.usuario AS Nombre 
                                 FROM Bitacora b  
                                 INNER JOIN Usuarios u ON b.Id_Usuario = u.Id_Usuario 
                                 WHERE CONVERT(date, b.Fecha, 103) >= CAST(GETDATE() - 3 AS DATE)
                                 ORDER BY CONVERT(date, b.Fecha, 103) DESC, b.Hora DESC";

                DataTable dt = LeerText(query);
                DesencriptarColumnaNombre(dt);
                return dt;
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
                string query = @"SELECT b.Id_Bitacora, b.Id_Usuario, b.Fecha, b.Hora, b.Actividad, b.Modulo, b.Criticidad, u.usuario AS Nombre 
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
                    parametros.Add(new SqlParameter("@Login", encriptar.EncriptarAES(login)));
                }
                if (modulo != "Todos" && !string.IsNullOrEmpty(modulo))
                {
                    query += " AND b.Modulo = @Modulo";
                    parametros.Add(new SqlParameter("@Modulo", modulo));
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

                DataTable dt = LeerText(query, parametros.ToArray());
                DesencriptarColumnaNombre(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error técnico en la base de datos al filtrar la bitácora. Detalle: " + ex.Message);
            }
        }

        private void DesencriptarColumnaNombre(DataTable dt)
        {
            if (dt == null || !dt.Columns.Contains("Nombre")) return;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Nombre"] != DBNull.Value && !string.IsNullOrEmpty(dr["Nombre"].ToString()))
                {
                    dr["Nombre"] = encriptar.DesencriptarAES(dr["Nombre"].ToString());
                }
            }
        }

        public void ActualizarDVH(int idBitacora, int dvh)
        {
            try
            {
                string sql = "UPDATE Bitacora SET DVH = @dvh WHERE Id_Bitacora = @id";
                SqlParameter[] p = {
                    new SqlParameter("@dvh", dvh),
                    new SqlParameter("@id", idBitacora)
                };
                EscribirText(sql, p);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar DVH en Bitacora: " + ex.Message);
            }
        }

        public DataTable ListarTodaBitacoraParaDVV()
        {
            try
            {
                string query = "SELECT Id_Bitacora, Id_Usuario, Actividad, Modulo, Criticidad, Fecha, Hora, DVH FROM Bitacora";
                return LeerText(query);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al leer bitácora completa para DVV: " + ex.Message);
            }
        }
    }
}