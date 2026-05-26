using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class Login : System.Web.UI.Page
    {
        BE.Usuario usuario = new BE.Usuario();
        BLL.BLLusuario bllusuario = new BLL.BLLusuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string usuarioIngresado = Request.Form["usuario"];
                string contrasenaIngresada = Request.Form["contrasena"];

                if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(contrasenaIngresada))
                {
                    MostrarAlertaJS("Por favor, ingrese sus credenciales.");
                    return;
                }
                try
                {
                    string mensaje = bllusuario.Login(usuarioIngresado, contrasenaIngresada);
                    Session["usuario"] = usuario;
                    Response.Redirect("~/UI/Inicio.aspx", false);
                }
                catch (Exception ex)
                {
                    MostrarAlertaJS(ex.Message);
                }
            }
        }
        private void MostrarAlertaJS(string mensaje)
        {
            string mensajeSeguro = mensaje.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            ClientScript.RegisterStartupScript(this.GetType(), "AlertaLogin", $"alert('{mensajeSeguro}');", true);
        }
    }
}