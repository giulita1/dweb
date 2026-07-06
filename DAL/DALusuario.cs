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
        public List<BE.Usuario> ObtenerTodos(string busqueda = "", bool soloBlockeados = false)
        {
            List<BE.Usuario> lista = new List<BE.Usuario>();

            string query = @"
                SELECT Id_Usuario, nombre, apellido, email, usuario, 
                       bloqueado, intentos, IdRol
                FROM Usuarios
                WHERE (@Busqueda = '' OR 
                       nombre LIKE '%' + @Busqueda + '%' OR 
                       apellido LIKE '%' + @Busqueda + '%' OR 
                       email LIKE '%' + @Busqueda + '%' OR
                       usuario LIKE '%' + @Busqueda + '%')
                  AND (@SoloBloqueados = 0 OR bloqueado = 1)
                ORDER BY apellido, nombre";

            SqlParameter[] p = {
        new SqlParameter("@Busqueda",       busqueda ?? ""),
        new SqlParameter("@SoloBloqueados", soloBlockeados ? 1 : 0)
    };

            DataTable dt = LeerText(query, p);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BE.Usuario
                {
                    Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                    Nombre = dr["nombre"].ToString(),
                    Apellido = dr["apellido"].ToString(),
                    Email = dr["email"].ToString(),
                    User = dr["usuario"].ToString(),
                    Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                    Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                    IdRol = Convert.ToInt32(dr["IdRol"])
                });
            }

            return lista;
        }

        public void DesbloquearUsuario(int idUsuario)
        {
            string query = @"UPDATE Usuarios 
                     SET bloqueado = 0, intentos = 0 
                     WHERE Id_Usuario = @Id";

            SqlParameter[] p = { new SqlParameter("@Id", idUsuario) };
            EscribirText(query, p);
        }
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

        public int RegistrarUsuario(Usuario usuario)
        {
            string query = "INSERT INTO USUARIOS (nombre, apellido, usuario, email, contrasena) " +
                           "OUTPUT INSERTED.Id_Usuario " +
                           "VALUES (@nombre, @apellido, @user, @email, @contrasena)";

            SqlParameter[] p = {
        new SqlParameter("@nombre", usuario.Nombre),
        new SqlParameter("@apellido", usuario.Apellido),
        new SqlParameter("@user", usuario.User),
        new SqlParameter("@email", usuario.Email),
        new SqlParameter("@contrasena", usuario.Contrasena)
    };

            try
            {
                return EscribirYDevolverId_62_RS(query, p);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar: " + ex.Message);
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

        public bool ExisteUsuario(string nombreUsuario)
        {
            string query = "SELECT COUNT(1) FROM Usuarios WHERE usuario = @u";

            Conectar();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@u", nombreUsuario);
            int resultado = (int)cmd.ExecuteScalar();
            Desconectar();
            return resultado > 0;
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
                        Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                        Nombre = dr["nombre"].ToString(),
                        Apellido = dr["apellido"].ToString(),
                        Email = dr["email"].ToString(),
                        User = dr["usuario"].ToString(),
                        Contrasena = dr["contrasena"].ToString(),
                        Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                        Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                        IdRol = dr["IdRol"] != DBNull.Value ? Convert.ToInt32(dr["IdRol"]) : 0,
                        IdIdioma = dr["IdIdioma"] != DBNull.Value ? Convert.ToInt32(dr["IdIdioma"]) : 0,
                        DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0
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
        public bool ExisteEmail(string email)
        {
            string query = "SELECT COUNT(1) FROM Usuarios WHERE email = @email";

            try
            {
                Conectar();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                int resultado = (int)cmd.ExecuteScalar();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en DAL al verificar email: " + ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void ActualizarContrasenaPorEmail(string email, string nuevaContrasenaHasheada)
        {
            string sql = "UPDATE Usuarios SET contrasena = @pass WHERE email = @email";
            SqlParameter[] p = {
        new SqlParameter("@pass", nuevaContrasenaHasheada),
        new SqlParameter("@email", email)
    };

            try
            {
                EscribirText(sql, p);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en DAL al actualizar contraseña: " + ex.Message);
            }
        }

        public void ActualizarDVH(int idUsuario, int dvh)
        {
            string sql = "UPDATE Usuarios SET DVH = @DVH WHERE Id_Usuario = @id";
            SqlParameter[] p = {
        new SqlParameter("@DVH", dvh),
        new SqlParameter("@id", idUsuario)
    };
            EscribirText(sql, p);
        }


        private BE.Usuario MapearObjetoUsuario(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new BE.Usuario
                {
                    Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                    Nombre = dr["nombre"].ToString(),
                    Apellido = dr["apellido"].ToString(),
                    Email = dr["email"].ToString(),
                    User = dr["usuario"].ToString(),
                    Contrasena = dr["contrasena"].ToString(),
                    Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                    Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                    IdRol = dr["IdRol"] != DBNull.Value ? Convert.ToInt32(dr["IdRol"]) : 0,
                    IdIdioma = dr["IdIdioma"] != DBNull.Value ? Convert.ToInt32(dr["IdIdioma"]) : 0,
                    DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0
                };
            }
            return null;
        }

        public BE.Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            string query = "SELECT * FROM Usuarios WHERE Id_Usuario = @id";
            SqlParameter[] p = { new SqlParameter("@id", idUsuario) };
            return MapearObjetoUsuario(LeerText(query, p));
        }
        public BE.Usuario ObtenerUsuarioPorNombre(string usuario)
        {
            string query = "SELECT * FROM Usuarios WHERE usuario = @u";
            SqlParameter[] p = { new SqlParameter("@u", usuario) };
            return MapearObjetoUsuario(LeerText(query, p));
        }

        public BE.Usuario ObtenerUsuarioPorEmail(string email)
        {
            string query = "SELECT * FROM Usuarios WHERE email = @email";
            SqlParameter[] p = { new SqlParameter("@email", email) };
            return MapearObjetoUsuario(LeerText(query, p));
        }

        public List<BE.Usuario> ObtenerTodosParaDVV()
        {
            List<BE.Usuario> lista = new List<BE.Usuario>();
            string query = "SELECT * FROM Usuarios";

            DataTable dt = LeerText(query);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BE.Usuario
                {
                    Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                    Nombre = dr["nombre"].ToString(),
                    Apellido = dr["apellido"].ToString(),
                    Email = dr["email"].ToString(),
                    User = dr["usuario"].ToString(),
                    Contrasena = dr["contrasena"].ToString(),
                    Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                    Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                    IdRol = dr["IdRol"] != DBNull.Value ? Convert.ToInt32(dr["IdRol"]) : 0,
                    IdIdioma = dr["IdIdioma"] != DBNull.Value ? Convert.ToInt32(dr["IdIdioma"]) : 0,
                    DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0
                });
            }
            return lista;
        }
    }
}