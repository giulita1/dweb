using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;
using desarrolloweb.BLL;
using SEG.Observer;
using SEG.singleton;

namespace desarrolloweb
{
    public partial class SiteMaster : MasterPage, IdiomaObserver
    {
        private readonly BLLusuario bLLusuario = new BLLusuario();
        private readonly BLLTraductor bllTraductor = new BLLTraductor();

        protected void Page_Init(object sender, EventArgs e)
        {
            SingletonSession.Instancia.SuscribirObservador(this);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            SingletonSession.Instancia.DesuscribirObservador(this);
        }

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
           /* else
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
            }*/
            else
            {
                BE.Usuario usuarioActual = (BE.Usuario)Session["usuario"];

                // Verificamos de forma dinámica los permisos de los módulos sensibles
                bool puedeVerBitacora = usuarioActual.Perfil?.TienePermiso("Eventos") == true; // O el nombre exacto de la patente que uses para la bitácora
                bool puedeVerDigitos = usuarioActual.Perfil?.TienePermiso("DigitoVerificador") == true;
                bool puedeRespaldar = usuarioActual.Perfil?.TienePermiso("Respaldos") == true;

                if (paginaActual.Contains("bitacora") && !puedeVerBitacora)
                {
                    Response.Redirect("~/UI/Inicio.aspx", true);
                    return;
                }
                if (paginaActual.Contains("digitoverficador") && !puedeVerDigitos)
                {
                    Response.Redirect("~/UI/Inicio.aspx", true);
                    return;
                }
                if (paginaActual.Contains("backuprestore") && !puedeRespaldar)
                {
                    Response.Redirect("~/UI/Inicio.aspx", true);
                    return;
                }
            }
            if (!IsPostBack)
            {
                CargarSelectorIdiomas();
            }
        }

        private void CargarSelectorIdiomas()
        {
            var idiomas = bllTraductor.ObtenerIdiomas();
            ddlIdiomaMaster.DataSource = idiomas;
            ddlIdiomaMaster.DataTextField = "Nombre";
            ddlIdiomaMaster.DataValueField = "Id";
            ddlIdiomaMaster.DataBind();

            Iidioma idiomaActivo = Session["IdiomaActual"] as Iidioma;
            if (idiomaActivo == null)
            {
                idiomaActivo = bllTraductor.ObtenerIdiomaDefault();
                Session["IdiomaActual"] = idiomaActivo;
            }

            if (idiomaActivo != null && ddlIdiomaMaster.Items.FindByValue(idiomaActivo.Id.ToString()) != null)
            {
                ddlIdiomaMaster.SelectedValue = idiomaActivo.Id.ToString();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Iidioma idiomaActivo = Session["IdiomaActual"] as Iidioma ?? bllTraductor.ObtenerIdiomaDefault();
            if (idiomaActivo != null)
            {
                SingletonSession.Instancia.IdiomaActual = idiomaActivo;
                UpdateLanguage(idiomaActivo);
            }
        }

        protected void ddlIdiomaMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSeleccionado = Convert.ToInt32(ddlIdiomaMaster.SelectedValue);
            Iidioma idiomaSeleccionado = bllTraductor.ObtenerIdiomas().FirstOrDefault(i => i.Id == idSeleccionado);

            if (idiomaSeleccionado != null)
            {
                Session["IdiomaActual"] = idiomaSeleccionado;
                SingletonSession.Instancia.CambiarIdioma(idiomaSeleccionado);
            }
        }

        public void UpdateLanguage(Iidioma idioma)
        {
            if (idioma == null) return;

            IDictionary<string, ITraduccion> traducciones = bllTraductor.ObtenerTraducciones(idioma);
            if (traducciones == null || traducciones.Count == 0) return;

            TraducirControles(this.Controls, traducciones);
        }

        private void TraducirControles(ControlCollection controles, IDictionary<string, ITraduccion> traducciones)
        {
            foreach (Control ctrl in controles)
            {
                if (!string.IsNullOrEmpty(ctrl.ID) && traducciones.ContainsKey(ctrl.ID))
                {
                    string textoTraducido = traducciones[ctrl.ID].Texto;

                    if (ctrl is Label lbl) lbl.Text = textoTraducido;
                    else if (ctrl is Button btn) btn.Text = textoTraducido;
                    else if (ctrl is LinkButton lnk) lnk.Text = textoTraducido;
                    else if (ctrl is CheckBox chk) chk.Text = textoTraducido;
                    else if (ctrl is RadioButton rdb) rdb.Text = textoTraducido;
                    else if (ctrl is Literal lit) lit.Text = textoTraducido;
                    else if (ctrl is TextBox txt) txt.Attributes["placeholder"] = textoTraducido;
                }

                if (ctrl.HasControls())
                {
                    TraducirControles(ctrl.Controls, traducciones);
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