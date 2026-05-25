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
            if (!IsPostBack)
            {
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
                Console.WriteLine(ex.Message);
            }
        }

        private void CargarDesplegablesDinamicos()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {

                string usuarioSel = string.IsNullOrEmpty(ddlUsuario.SelectedValue) ? "Todos" : ddlUsuario.SelectedValue;
                string moduloSel = string.IsNullOrEmpty(ddlModulo.SelectedValue) ? "Todos" : ddlModulo.SelectedValue;
                string eventoSel = string.IsNullOrEmpty(ddlEvento.SelectedValue) ? "Todos" : ddlEvento.SelectedValue;
                string criticidadSel = string.IsNullOrEmpty(ddlCriticidad.SelectedValue) ? "Todos" : ddlCriticidad.SelectedValue;
                DateTime fechaDesde;
                DateTime fechaHasta;

                if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesde))
                {
                    fechaDesde = new DateTime(2000, 1, 1);
                }

                if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHasta))
                {
                    fechaHasta = DateTime.Now;
                }

                DataTable resultado = bitacoraBLL.FiltrarBitacora(usuarioSel, moduloSel, eventoSel, criticidadSel, fechaDesde, fechaHasta);
                gvBitacora.DataSource = resultado;
                gvBitacora.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

    }
}