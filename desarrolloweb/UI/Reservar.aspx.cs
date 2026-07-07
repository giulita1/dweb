using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using BE;

namespace desarrolloweb.UI
{
	public partial class Reservar : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string llegada = Request.QueryString["llegada"];
                string salida = Request.QueryString["salida"];
                string huespedes = Request.QueryString["huespedes"];

                if (!string.IsNullOrEmpty(llegada) && !string.IsNullOrEmpty(salida))
                    BuscarHabitaciones(llegada, salida, huespedes);
            }
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = $"lbl-mensaje {tipo}";
            lblMensaje.Visible = true;
            pnlResultados.Visible = false;
        }

        private void BuscarHabitaciones(string llegadaStr, string salidaStr, string huespedesStr)
        {
            if (!DateTime.TryParse(llegadaStr, out DateTime llegada) ||
                !DateTime.TryParse(salidaStr, out DateTime salida))
            {
                MostrarMensaje("Por favor ingresá fechas válidas.", "error");
                return;
            }

            if (llegada >= salida)
            {
                MostrarMensaje("La fecha de salida debe ser posterior a la de llegada.", "error");
                return;
            }

            if (llegada < DateTime.Today)
            {
                MostrarMensaje("La fecha de llegada no puede ser en el pasado.", "error");
                return;
            }

            int huespedes = 1;
            int.TryParse(huespedesStr, out huespedes);
            if (huespedes < 1) huespedes = 1;

            BLLHabitacion bll = new BLLHabitacion();
            List<Habitacion> habitaciones = bll.ObtenerHabitacionesDisponibles(llegada, salida, huespedes);

            pnlResultados.Visible = true;

            int noches = (salida - llegada).Days;
            lblSubtitulo.Text = $"Estadía de <strong>{noches} noche{(noches > 1 ? "s" : "")}</strong> · " +
                                $"{llegada:dd/MM/yyyy} → {salida:dd/MM/yyyy} · {huespedes} huésped{(huespedes > 1 ? "es" : "")}";

            if (habitaciones == null || habitaciones.Count == 0)
            {
                rptHabitaciones.Visible = false;
                pnlSinResultados.Visible = true;
            }
            else
            {
                pnlSinResultados.Visible = false;
                rptHabitaciones.Visible = true;
                rptHabitaciones.DataSource = habitaciones;
                rptHabitaciones.DataBind();
            }

            Session["FechaLlegada"] = llegada;
            Session["FechaSalida"] = salida;
            Session["Huespedes"] = huespedes;
        }
    }
}