using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DAL;
using desarrolloweb.BE;

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
    }
}