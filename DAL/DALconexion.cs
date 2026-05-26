using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALconexion
    {
        protected SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=hotel_;Integrated Security=True");

       // protected SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=hotel_;Integrated Security=True;TrustServerCertificate=True");


        private SqlTransaction transaccion;
        public void Conectar()
        {
            try
            {
           
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Desconectar()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int EscribirText(string query, SqlParameter[] parametros)
        {
            Conectar();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            if (parametros != null) cmd.Parameters.AddRange(parametros);

            transaccion = con.BeginTransaction();
            cmd.Transaction = transaccion;
            try
            {
                int filasafectadas = cmd.ExecuteNonQuery();
                transaccion.Commit();
                return filasafectadas;
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                throw new Exception("Error al ejecutar la consulta: " + ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public DataTable LeerText(string consulta, SqlParameter[] parametros = null)
        {
            DataTable tabla = new DataTable();
            Conectar();

            try
            {
                using (SqlCommand cmd = new SqlCommand(consulta, con))
                {
                    cmd.CommandType = CommandType.Text;

                    if (parametros != null)
                    {
                        cmd.Parameters.AddRange(parametros);
                    }

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
                return tabla;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la lectura de datos (SQL): " + ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }

    }
}
