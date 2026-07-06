using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using desarrolloweb.BLL;
using SEG.singleton;

namespace desarrolloweb
{
    public partial class SiteMaster : MasterPage
    {
        BLLusuario bLLusuario = new BLLusuario();

        protected void Page_Load(object sender, EventArgs e)
        {
            string paginaActual = Request.Url.AbsolutePath.ToLower();
            bool logueado = Session["usuario"] != null;

            if (paginaActual.Contains(".css") ||
                paginaActual.Contains(".js") ||
                paginaActual.Contains(".woff") ||
                paginaActual.Contains(".woff2") ||
                paginaActual.Contains(".png") ||
                paginaActual.Contains(".jpg"))
            {
                return;
            }

            bool esPaginaPublica = paginaActual.Contains("inicio") ||
                                   paginaActual.Contains("login") ||
                                   paginaActual.Contains("registro") ||
                                   paginaActual == "/" ||
                                   paginaActual.EndsWith("default");

            if (!logueado)
            {
                if (!esPaginaPublica)
                {

                    Response.Redirect("~/UI/Login.aspx", true);
                    return;
                }
            }
            else
            {
                BE.Usuario usuarioActual = (BE.Usuario)Session["usuario"];
                bool esAuditor = (usuarioActual.Id_Usuario == 0);

                if (!esAuditor)
                {
                    if (paginaActual.Contains("bitacora") ||
                        paginaActual.Contains("digitoverficador") ||
                        paginaActual.Contains("backuprestore"))
                    {
                        Response.Redirect("~/UI/Inicio.aspx", true);
                        return;
                    }
                }
            }

        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
            }
            bLLusuario.logout();
            Response.Redirect("~/UI/Inicio.aspx", false);
        }

        protected void btnBitacora_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/UI/Bitacora.aspx"), false);
        }
    }
}