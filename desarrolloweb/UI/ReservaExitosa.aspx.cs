using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class ReservaExitosa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
                return;
            }

            if (!IsPostBack)
                CargarDatos();

        }

        private void CargarDatos()
        {
            // Leer los datos que quedaron en sesión tras la reserva
            // Si no hay datos redirigir
            if (Session["UltimaReserva_Nombre"] == null)
            {
                lblNombreHab.InnerText = "Habitación reservada";
                lblLlegada.InnerText = "—";
                lblSalida.InnerText = "—";
                lblTotal.InnerText = "—";
                return;
            }

            lblNombreHab.InnerText = Session["UltimaReserva_Nombre"].ToString();
            lblLlegada.InnerText = $"Llegada: {((DateTime)Session["UltimaReserva_Llegada"]):dd/MM/yyyy}";
            lblSalida.InnerText = $"Salida: {((DateTime)Session["UltimaReserva_Salida"]):dd/MM/yyyy}";
            lblTotal.InnerText = $"Total pagado: {((double)Session["UltimaReserva_Total"]):C0}";

            // Limpiar
            Session.Remove("UltimaReserva_Nombre");
            Session.Remove("UltimaReserva_Llegada");
            Session.Remove("UltimaReserva_Salida");
            Session.Remove("UltimaReserva_Total");
        }
    }
}