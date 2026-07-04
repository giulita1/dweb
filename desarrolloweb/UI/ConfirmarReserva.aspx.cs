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
    public partial class ConfirmarReserva : System.Web.UI.Page
    {
        private const double PRECIO_DESAYUNO = 5000;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirigir si no está logueado
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
                return;
            }

            if (!IsPostBack)
                CargarResumen();
        }

        private void CargarResumen()
        {
            // Leer sesión
            int habitacionId = (int)Session["HabitacionId"];
            DateTime llegada = (DateTime)Session["FechaLlegada"];
            DateTime salida = (DateTime)Session["FechaSalida"];
            int huespedes = (int)Session["Huespedes"];
            bool conDesayuno = Request.QueryString["desayuno"] == "true";

            int noches = (salida - llegada).Days;

            // Traer habitación
            BLLHabitacion bll = new BLLHabitacion();
            Habitacion hab = bll.ObtenerPorId(habitacionId);

            // Imagen y badge
            imgHabitacion.Src = ResolveUrl("~/img/" + hab.ImagenUrl);
            lblTipo.InnerText = hab.Tipo;
            lblNombre.InnerText = hab.Nombre;

            // Fechas
            lblLlegada.InnerText = llegada.ToString("dd 'de' MMMM yyyy",
                                    new System.Globalization.CultureInfo("es-AR"));
            lblSalida.InnerText = salida.ToString("dd 'de' MMMM yyyy",
                                    new System.Globalization.CultureInfo("es-AR"));

            // Detalles
            lblNoches.InnerText = $"{noches} noche{(noches > 1 ? "s" : "")}";
            lblHuespedes.InnerText = $"{huespedes} huésped{(huespedes > 1 ? "es" : "")}";

            // Precios
            double precioHab = hab.PrecioPorNoche * noches;
            double precioDesay = conDesayuno ? PRECIO_DESAYUNO * huespedes * noches : 0;
            double total = precioHab + precioDesay;


            lblPrecioHab.InnerText = precioHab.ToString("C0", new System.Globalization.CultureInfo("es-AR"));

            if (conDesayuno)
            {
                pnlDesayuno.Visible = true;
                lblDescDesayuno.InnerText = $"Desayuno × {huespedes} pax × {noches} noche{(noches > 1 ? "s" : "")}";
                lblPrecioDesayuno.InnerText = precioDesay.ToString("C0", new System.Globalization.CultureInfo("es-AR"));
            }

            lblTotal.InnerText = total.ToString("C0", new System.Globalization.CultureInfo("es-AR"));


            // Guardar en sesión para usarlo al confirmar
            Session["Total"] = total;
            Session["ConDesayuno"] = conDesayuno;
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            // Validaciones básicas del lado servidor
            // (los valores vienen del JS ya formateados)
            if (!ValidarFormulario())
                return;

            //Crear la reserva en la BD
            BLLReserva bllreserva = new BLLReserva();

            Reserva reserva = new Reserva();
            reserva.Hab.Id_Habitacion = (int)Session["HabitacionId"];
            reserva.Usuario_.Id_Usuario = ((Usuario)Session["usuario"]).Id_Usuario;
            reserva.FechaLlegada = (DateTime)Session["FechaLlegada"];
            reserva.FechaSalida = (DateTime)Session["FechaSalida"];
            reserva.Huespedes = (int)Session["Huespedes"];
            reserva.IncluyeDesayuno = (bool)Session["ConDesayuno"];
            reserva.Total = (double)Session["Total"];
            reserva.Estado = "Confirmada";
            

            bllreserva.CrearReserva(reserva);

            // Limpiar sesión de carrito
            Session.Remove("HabitacionId");
            Session.Remove("FechaLlegada");
            Session.Remove("FechaSalida");
            Session.Remove("Huespedes");
            Session.Remove("ConDesayuno");
            Session.Remove("Total");

            Response.Redirect("~/UI/ReservaExitosa.aspx");
        }

        private bool ValidarFormulario()
        {
            // Validación mínima — el JS ya formatea los campos
            // En un sistema real nunca confíes solo en el cliente
            lblError.Visible = false;
            return true;
        }
    }
}