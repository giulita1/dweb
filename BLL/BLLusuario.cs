using desarrolloweb.BE;
using desarrolloweb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace desarrolloweb.BLL
{
    public class BLLusuario
    {

        DALusuario Dal = new DALusuario();
        public void VerificarExistencia(Usuario usuario) { 
            
            Dal.VerificarExistencia(usuario);
        
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            usuario.Contrasena = EncriptarContraseña(usuario.Contrasena);
            Dal.RegistrarUsuario(usuario);
        }

        public string EncriptarContraseña(string contrasena)
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
    }
}