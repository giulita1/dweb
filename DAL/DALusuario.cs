using DAL;
using desarrolloweb.BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace desarrolloweb.DAL
{
    public class DALusuario : DALconexion
    {
        public bool VerificarExistencia(Usuario usuario)
        {
            string query = "SELECT COUNT(*) FROM USUARIOS WHERE email = @email OR usuario = @user";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@user", usuario.User);
            try
            {
                Conectar();
                int existe = (int)cmd.ExecuteScalar();
                return existe > 0;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
            finally
            {
            Desconectar();
            }

        
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            string query = "INSERT INTO USUARIOS (nombre, apellido, usuario, email, contrasena) " +
                           "VALUES (@nombre, @apellido, @user, @email, @contrasena)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@user", usuario.User);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);

            try
            {
                Conectar();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }
        public static string EncriptarContraseña(string contrasena)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contrasena));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public BE.Usuario ValidarAcceso(string usuario, string contrasena)
        {

            try
            {
                string query = "SELECT * FROM Usuarios WHERE usuario = @u AND contrasena = @p";
                SqlParameter[] p = {
            new SqlParameter("@u", usuario),
            new SqlParameter("@p", contrasena)
        };

                DataTable dt = LeerText(query, p);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    return new BE.Usuario
                    {
                        Cod_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                        Nombre = dr["nombre"].ToString(),
                        Apellido = dr["apellido"].ToString(),
                        Email = dr["email"].ToString(),
                        User = dr["usuario"].ToString(),
                        Contrasena = dr["contrasena"].ToString(),
                        Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                        Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0
                    };
                }
                return null;
            }
            catch (Exception ex) { throw new Exception("Error en DAL: " + ex.Message); }
        }
        public void BloquearUsuario_62_RS(string usuario)
        {
            string sql = "UPDATE Usuarios SET bloqueado = @est WHERE usuario = @u";
            SqlParameter[] p = {
        new SqlParameter("@est", true),
        new SqlParameter("@u", usuario)
        };
            EscribirText(sql, p);
        }

        public int SumarIntentosFallidos(string usuario)
        {
            try
            {
                string query = @"UPDATE Usuarios 
                         SET intentos = intentos + 1 
                         OUTPUT INSERTED.intentos 
                         WHERE usuario = @u";

                SqlParameter[] p = {
            new SqlParameter("@u", usuario)
        };
                DataTable dt = LeerText(query, p);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt16(dt.Rows[0]["intentos"]);
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en DAL al sumar intentos: " + ex.Message);
            }
        }
        public void ResetearIntentos(string usuario)
        {
            try
            {
                string sql = "UPDATE Usuarios SET intentos = 0 WHERE usuario = @u";

                SqlParameter[] p = {
            new SqlParameter("@u", usuario)
        };
                EscribirText(sql, p);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en DAL al resetear los intentos: " + ex.Message);
            }
        }

    }
}