using desarrolloweb.BE;
using desarrolloweb.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class Registro : System.Web.UI.Page
    {
        BLLusuario bllusuario = new BLLusuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Usuario usuario = new Usuario();
                usuario.Email = Request.Form["email"];
                usuario.Nombre = Request.Form["nombre"];
                usuario.Apellido = Request.Form["apellido"];
                usuario.User = Request.Form["usuario"];
                usuario.Contrasena = Request.Form["contrasena"];
                string confirmar = Request.Form["confirmarcontrasena"];

                bool errores = false;

                if (string.IsNullOrEmpty(usuario.Nombre))
                {
                    MostrarError(lblNombre, "El nombre es obligatorio.");
                    errores = true;
                }

                if (string.IsNullOrEmpty(usuario.Apellido))
                {
                    MostrarError(lblApellido, "El apellido es obligatorio.");
                    errores = true;
                }

                if (string.IsNullOrEmpty(usuario.Email))
                {
                    MostrarError(lblEmail, "El email es obligatorio.");
                    errores = true;
                }
                else if (!ValidarEmail(usuario.Email))
                {
                    MostrarError(lblEmail, "El formato del email no es válido.");
                    errores = true;
                }

                if (string.IsNullOrEmpty(usuario.User))
                {
                    MostrarError(lblUsuario, "El usuario es obligatorio.");
                    errores = true;
                }

                if (string.IsNullOrEmpty(usuario.Contrasena))
                {
                    MostrarError(lblContra, "La contraseña es obligatoria.");
                    errores = true;
                }

                if (string.IsNullOrEmpty(confirmar))
                {
                    MostrarError(lblConfirmar, "Confirmá tu contraseña.");
                    errores = true;
                }

                else if (usuario.Contrasena != confirmar)
                {
                    MostrarError(lblConfirmar, "Las contraseñas no coinciden.");
                    errores = true;
                }

                if (errores)
                {
                    return;
                }

                try
                {
                    if (bllusuario.VerificarExistencia(usuario))
                    {
                        MostrarError(lblEmail, "El email ya está registrado.");
                        MostrarError(lblUsuario, "El usuario ya está registrado.");
                        return;
                    }

                    bllusuario.RegistrarUsuario(usuario);
                    Response.Redirect("Inicio.aspx");
                }
                catch (Exception)
                {
                    MostrarError(lblNombre, "Error al registrarse. Intentá de nuevo.");
                }
            }
        }
        private void MostrarError(Label lbl, string mensaje)
        {
            lbl.Text = mensaje;
            lbl.Visible = true;
        }

        private bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}