using BE;
using BLL;
using desarrolloweb.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class MisReservas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
                return;
            }

            if (!IsPostBack)
                CargarReservas();
        }

        private void CargarReservas()
        {
            Usuario usuario = (Usuario)Session["usuario"];
            BLLReserva bll = new BLLReserva();

            List<Reserva> reservas = bll.ObtenerReservasPorUsuario(usuario.Id_Usuario);

            if (reservas == null || reservas.Count == 0)
            {
                pnlSinReservas.Visible = true;
                rptReservas.Visible = false;
            }
            else
            {
                pnlSinReservas.Visible = false;
                rptReservas.Visible = true;
                rptReservas.DataSource = reservas;
                rptReservas.DataBind();
            }
        }

        protected void rptReservas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Cancelar")
            {
                Usuario usuario = (Usuario)Session["usuario"];
                int idReserva = Convert.ToInt32(e.CommandArgument);

                BLLReserva bll = new BLLReserva();
                bll.CancelarReserva(idReserva, usuario.Id_Usuario);

                CargarReservas();
            }
        }
    }
}