using BE;
using DAL;
using desarrolloweb.BE;
using SEG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
                WHERE (@SoloBloqueados = 0 OR bloqueado = 1)";

            SqlParameter[] p = {
                new SqlParameter("@SoloBloqueados", soloBlockeados ? 1 : 0)
            };

            DataTable dt = LeerText(query, p);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BE.Usuario
                {
                    Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                    Nombre = encriptar.DesencriptarAES(dr["nombre"].ToString()),
                    Apellido = encriptar.DesencriptarAES(dr["apellido"].ToString()),
                    Email = encriptar.DesencriptarAES(dr["email"].ToString()),
                    User = encriptar.DesencriptarAES(dr["usuario"].ToString()),
                    Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                    Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                    Perfil = new Perfil
                    {
                        Id = dr["IdRol"] != DBNull.Value ? Convert.ToInt32(dr["IdRol"]) : 0
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                string term = busqueda.Trim().ToLower();
                lista = lista.Where(u =>
                    (u.Nombre != null && u.Nombre.ToLower().Contains(term)) ||
                    (u.Apellido != null && u.Apellido.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.User != null && u.User.ToLower().Contains(term))
                ).ToList();
            }

            return lista.OrderBy(u => u.Apellido).ThenBy(u => u.Nombre).ToList();
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

            cmd.Parameters.AddWithValue("@email", encriptar.EncriptarAES(usuario.Email));
            cmd.Parameters.AddWithValue("@user", encriptar.EncriptarAES(usuario.User));
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
                           "VALUES (@nombre, @apellido, @user, @email, @contrasena); SELECT SCOPE_IDENTITY();";

            SqlParameter[] p = {
                new SqlParameter("@nombre", encriptar.EncriptarAES(usuario.Nombre)),
                new SqlParameter("@apellido", encriptar.EncriptarAES(usuario.Apellido)),
                new SqlParameter("@user", encriptar.EncriptarAES(usuario.User)),
                new SqlParameter("@email", encriptar.EncriptarAES(usuario.Email)),
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
            cmd.Parameters.AddWithValue("@u", encriptar.EncriptarAES(nombreUsuario));
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
                    new SqlParameter("@u", encriptar.EncriptarAES(usuario)),
                    new SqlParameter("@p", contrasena)
                };

                DataTable dt = LeerText(query, p);
                return MapearObjetoUsuario(dt);
            }
            catch (Exception ex) { throw new Exception("Error en DAL: " + ex.Message); }
        }

        public void BloquearUsuario_62_RS(string usuario)
        {
            string sql = "UPDATE Usuarios SET bloqueado = @est WHERE usuario = @u";
            SqlParameter[] p = {
                new SqlParameter("@est", true),
                new SqlParameter("@u", encriptar.EncriptarAES(usuario))
            };
            EscribirText(sql, p);
        }

        public int SumarIntentosFallidos(string usuario)
        {
            try
            {
                string usuarioCifrado = encriptar.EncriptarAES(usuario);

                string queryUpdate = @"UPDATE Usuarios 
                                       SET intentos = ISNULL(intentos, 0) + 1 
                                       WHERE usuario = @u";

                SqlParameter[] p1 = { new SqlParameter("@u", usuarioCifrado) };
                EscribirText(queryUpdate, p1);

                string querySelect = "SELECT intentos FROM Usuarios WHERE usuario = @u";
                SqlParameter[] p2 = { new SqlParameter("@u", usuarioCifrado) };
                DataTable dt = LeerText(querySelect, p2);

                if (dt != null && dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0]["intentos"]);

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
                    new SqlParameter("@u", encriptar.EncriptarAES(usuario))
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
                cmd.Parameters.AddWithValue("@email", encriptar.EncriptarAES(email));
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
                new SqlParameter("@email", encriptar.EncriptarAES(email))
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
                    Nombre = encriptar.DesencriptarAES(dr["nombre"].ToString()),
                    Apellido = encriptar.DesencriptarAES(dr["apellido"].ToString()),
                    Email = encriptar.DesencriptarAES(dr["email"].ToString()),
                    User = encriptar.DesencriptarAES(dr["usuario"].ToString()),
                    Contrasena = dr["contrasena"].ToString(),
                    Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                    Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                    Perfil = new Perfil
                    {
                        Id = dr["IdRol"] != DBNull.Value ? Convert.ToInt32(dr["IdRol"]) : 0
                    },
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
            SqlParameter[] p = { new SqlParameter("@u", encriptar.EncriptarAES(usuario)) };
            return MapearObjetoUsuario(LeerText(query, p));
        }

        public BE.Usuario ObtenerUsuarioPorEmail(string email)
        {
            string query = "SELECT * FROM Usuarios WHERE email = @email";
            SqlParameter[] p = { new SqlParameter("@email", encriptar.EncriptarAES(email)) };
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
                    Nombre = encriptar.DesencriptarAES(dr["nombre"].ToString()),
                    Apellido = encriptar.DesencriptarAES(dr["apellido"].ToString()),
                    Email = encriptar.DesencriptarAES(dr["email"].ToString()),
                    User = encriptar.DesencriptarAES(dr["usuario"].ToString()),
                    Contrasena = dr["contrasena"].ToString(),
                    Bloqueado = dr["bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["bloqueado"]),
                    Intentos = dr["intentos"] != DBNull.Value ? Convert.ToInt16(dr["intentos"]) : (short)0,
                    Perfil = new Perfil
                    {
                        Id = dr["IdRol"] != DBNull.Value ? Convert.ToInt32(dr["IdRol"]) : 0
                    },
                    IdIdioma = dr["IdIdioma"] != DBNull.Value ? Convert.ToInt32(dr["IdIdioma"]) : 0,
                    DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0
                });
            }
            return lista;
        }
    }
}