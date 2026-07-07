using desarrolloweb.BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class Bitacora : System.Web.UI.Page
    {
        private BLL.BLLusuario usuariobll = new BLL.BLLusuario();
        private BLL.BLLbitacora bitacoraBLL = new BLL.BLLbitacora();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                lblMensaje.Text = "";
                CargarDesplegablesDinamicos();
                CargarGrillaInicial();

            }
            Page.DataBind();
        }
        private void CargarGrillaInicial()
        {
            try
            {
                DataTable dt = bitacoraBLL.ListarBitacora();
                gvBitacora.DataSource = dt;
                gvBitacora.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar la bitácora: " + ex.Message;
            }
        }

        private void CargarDesplegablesDinamicos()
        {
            try
            {
                List<BE.Usuario> usuarios = usuariobll.ObtenerTodosParaDVV();

                ddlUsuario.Items.Clear();
                ddlUsuario.Items.Add(new ListItem("-- Todos los usuarios --", "Todos"));

                foreach (BE.Usuario usu in usuarios)
                {
                    ddlUsuario.Items.Add(new ListItem(usu.User, usu.User));
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar filtros dinámicos: " + ex.Message;
            }
        }
        private void CargarGrillaConFiltros()
        {
            try
            {
                string usuarioSel = ddlUsuario.SelectedValue;
                if (string.IsNullOrEmpty(usuarioSel)) usuarioSel = "Todos";

                string moduloSel = ddlModulo.SelectedValue;
                if (string.IsNullOrEmpty(moduloSel)) moduloSel = "Todos";

                string eventoSel = ddlEvento.SelectedValue;
                if (string.IsNullOrEmpty(eventoSel)) eventoSel = "Todos";

                string criticidadSel = ddlCriticidad.SelectedValue;
                if (string.IsNullOrEmpty(criticidadSel)) criticidadSel = "Todos";

                DateTime fechaDesde;
                DateTime fechaHasta;

                if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesde))
                {
                    fechaDesde = DateTime.Now.AddDays(-3);
                    txtFechaDesde.Text = fechaDesde.ToString("yyyy-MM-dd");
                }

                if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHasta))
                {
                    fechaHasta = DateTime.Now;
                    txtFechaHasta.Text = fechaHasta.ToString("yyyy-MM-dd");
                }

                DataTable resultado = bitacoraBLL.FiltrarBitacora(usuarioSel, moduloSel, eventoSel, criticidadSel, fechaDesde, fechaHasta);
                gvBitacora.DataSource = resultado;
                gvBitacora.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar los registros de bitácora: " + ex.Message;
            }
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            gvBitacora.PageIndex = 0;
            CargarGrillaConFiltros();

        }
        protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBitacora.PageIndex = e.NewPageIndex;
            CargarGrillaConFiltros();
        }

    }
}