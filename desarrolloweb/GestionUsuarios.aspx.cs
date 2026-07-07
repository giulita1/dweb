using desarrolloweb.BE;
using desarrolloweb.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb
{
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null ||
                ((Usuario)Session["usuario"]).IdRol != 1)
            {
                Response.Redirect("~/UI/Inicio.aspx");
                return;
            }

            if (!IsPostBack)
                CargarUsuarios();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }
        private void CargarUsuarios()
        {
            string busqueda = Request.QueryString["busqueda"] ?? "";
            bool soloBloqueados = Request.QueryString["bloqueados"] == "1";

            BLLusuario bll = new BLLusuario();
            List<Usuario> usuarios = bll.ObtenerTodos(busqueda, soloBloqueados);

            int total = usuarios.Count;
            lblContador.Text = total == 0 ? "Sin resultados" : $"{total} usuario{(total > 1 ? "s" : "")} encontrado{(total > 1 ? "s" : "")}";

            pnlTabla.Visible = total > 0;
            pnlSinResultados.Visible = total == 0;

            rptUsuarios.DataSource = usuarios;
            rptUsuarios.DataBind();
        }
        protected void rptUsuarios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Desbloquear")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                BLLusuario bll = new BLLusuario();
                bll.DesbloquearUsuario(idUsuario);
                CargarUsuarios();
            }
        }

    }
}