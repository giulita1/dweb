using desarrolloweb.BE;
using desarrolloweb.DAL;
using SEG;
using SEG.singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;


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
        public string RecuperarContrasena(string email)
        {
            try
            {
                if (!Dal.ExisteEmail(email))
                {
                    throw new Exception("El correo electrónico ingresado no se encuentra registrado.");
                }

                string nuevaContrasena = GenerarContrasenaAleatoria(8);
                string passHasheada = SEG.encriptar.EncriptarContraseña(nuevaContrasena);
                Dal.ActualizarContrasenaPorEmail(email, passHasheada);
                EnviarEmailRecuperacion(email, nuevaContrasena);
                bllBitacora.InsertarBitacora(0, $"Recuperación de contraseña solicitada para email: {email}", "Seguridad", "Media");

                return "Se ha enviado una nueva contraseña a tu correo electrónico.";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerarContrasenaAleatoria(int longitud)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < longitud; i++)
            {
                res.Append(caracteres[rnd.Next(caracteres.Length)]);
            }
            return res.ToString();
        }

        private void EnviarEmailRecuperacion(string emailDestino, string nuevaContrasena)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ReservasHotelDweb@gmail.com");
                mail.To.Add(emailDestino);
                mail.Subject = "Recuperación de Contraseña - Tu Sistema";
                mail.Body = $"Hola,\n\nSe ha solicitado restablecer tu contraseña.\n\nTu nueva contraseña temporal es: {nuevaContrasena}\n\nTe recomendamos iniciar sesión y cambiarla desde tu perfil lo antes posible.\n\nSaludos.";
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); // Puerto estándar para TLS
                                                                         // Si usas Gmail, necesitas generar una "Contraseña de aplicación" en tu cuenta de Google. No sirve tu contraseña habitual.
                smtp.Credentials = new NetworkCredential("ReservasHotelDweb@gmail.com", "cmxb elyp otyr rcys");
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("La contraseña se cambió, pero hubo un error al enviar el correo: " + ex.Message);
            }
        }
    }
}