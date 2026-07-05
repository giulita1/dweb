using desarrolloweb.BE;
using desarrolloweb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SEG;
using SEG.singleton;

namespace desarrolloweb.BLL
{
    public class BLLusuario
    {

        DALusuario Dal = new DALusuario();
        BLLbitacora bllBitacora = new BLLbitacora();

        public List<BE.Usuario> ObtenerTodos(string busqueda = "", bool soloBloqueados = false)
        {
            return Dal.ObtenerTodos(busqueda, soloBloqueados);
        }

        public void DesbloquearUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Usuario inválido.");

            Dal.DesbloquearUsuario(idUsuario);
            bllBitacora.InsertarBitacora(idUsuario, $"Usuario desbloqueado: {idUsuario}", "Seguridad", "Media");
        }
        public string Login(string Usuario, string Contrasena)
        {
            try
            {
                if (SingletonSession.Instancia.EstaAutenticado())
                {
                    throw new Exception("Ya existe una sesión activa. Debe cerrar la sesión actual para iniciar una nueva.");
                }

                string passHasheada = SEG.encriptar.EncriptarContraseña(Contrasena);
                BE.Usuario usuario = Dal.ValidarAcceso(Usuario, passHasheada);

                if (usuario == null)
                {
                    bool existeUsuario = Dal.ExisteUsuario(Usuario);

                    if (!existeUsuario)
                    {
                        throw new Exception("Credenciales incorrectas.");
                    }

                    int intentosResultantes = Dal.SumarIntentosFallidos(Usuario);

                    if (intentosResultantes >= 3)
                    {
                        BloquearUsuarioPorNombre(Usuario);
                        bllBitacora.InsertarBitacora(0, $"Bloqueo preventivo de cuenta: {Usuario}", "Seguridad", "Alta");
                        throw new Exception("Has superado los 3 intentos. El usuario ha sido bloqueado por seguridad.");
                    }

                    throw new Exception($"Credenciales incorrectas. Intento {intentosResultantes} de 3.");
                }


                if (usuario.Bloqueado)
                {
                    throw new Exception("Cuenta bloqueada.");
                }

                int usulogin = usuario.Id_Usuario;

                SingletonSession.Instancia.IniciarSesion(usuario);
                Dal.ResetearIntentos(Usuario);

                int idUsuarioActual = SingletonSession.Instancia.Usuario.Id_Usuario;
                bllBitacora.InsertarBitacora(idUsuarioActual, "Inicio de sesión exitoso", "Seguridad", "1");

                return "¡Bienvenido/a!";
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void logout()
        {
            if (!SingletonSession.Instancia.EstaAutenticado())

            {
                throw new Exception("No hay una sesión activa.");
            }

            else

            {
                SingletonSession.Instancia.CerrarSesion();
            }
        }
        public bool VerificarExistencia(Usuario usuario) { 
            
            return Dal.VerificarExistencia(usuario);
        
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            usuario.Contrasena = SEG.encriptar.EncriptarContraseña(usuario.Contrasena);
            Dal.RegistrarUsuario(usuario);
        }

        private void BloquearUsuarioPorNombre(string Usuario)
        {
            Dal.BloquearUsuario_62_RS(Usuario);
        }
    }
}