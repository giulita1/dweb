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
            // Leer desde QueryString en vez de Session
            int habitacionId = Convert.ToInt32(Request.QueryString["hab"]);
            bool conDesayuno = Request.QueryString["desayuno"] == "true";

            // Parsear fechas — vienen como strings del JS
            DateTime llegada, salida;
            if (!DateTime.TryParse(Request.QueryString["llegada"], out llegada) ||
                !DateTime.TryParse(Request.QueryString["salida"], out salida))
            {
                Response.Redirect("~/UI/Reservar.aspx");
                return;
            }

            int huespedes = Convert.ToInt32(Request.QueryString["huespedes"]);
            int noches = (salida - llegada).Days;

            // Traer habitación desde BD para tener el precio real
            BLLHabitacion bll = new BLLHabitacion();
            Habitacion hab = bll.ObtenerPorId(habitacionId);

            // Imagen y badge
            imgHabitacion.Src = ResolveUrl("~/img/" + hab.ImagenUrl);
            lblTipo.InnerText = hab.Tipo;
            lblNombre.InnerText = hab.Nombre;

            // Fechas
            var cultura = new System.Globalization.CultureInfo("es-AR");
            lblLlegada.InnerText = llegada.ToString("dd 'de' MMMM yyyy", cultura);
            lblSalida.InnerText = salida.ToString("dd 'de' MMMM yyyy", cultura);

            // Detalles
            lblNoches.InnerText = $"{noches} noche{(noches > 1 ? "s" : "")}";
            lblHuespedes.InnerText = $"{huespedes} huésped{(huespedes > 1 ? "es" : "")}";

            // Precios
            double precioHab = hab.PrecioPorNoche * noches;
            double precioDesay = conDesayuno ? PRECIO_DESAYUNO * huespedes * noches : 0;
            double total = precioHab + precioDesay;

            lblDescHab.InnerText = $"Habitación × {noches} noche{(noches > 1 ? "s" : "")}";
            lblPrecioHab.InnerText = precioHab.ToString("C0", cultura);

            if (conDesayuno)
            {
                pnlDesayuno.Visible = true;
                lblDescDesayuno.InnerText = $"Desayuno × {huespedes} pax × {noches} noche{(noches > 1 ? "s" : "")}";
                lblPrecioDesayuno.InnerText = precioDesay.ToString("C0", cultura);
            }

            lblTotal.InnerText = total.ToString("C0", cultura);

            // Guardar en Session para usar al pagar
            Session["Id_Habitacion"] = habitacionId;
            Session["FechaLlegada"] = llegada;
            Session["FechaSalida"] = salida;
            Session["Huespedes"] = huespedes;
            Session["ConDesayuno"] = conDesayuno;
            Session["Total"] = total;
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;

            // Leer de Session antes de remover
            int idHabitacion = (int)Session["Id_Habitacion"];
            DateTime fechaLleg = (DateTime)Session["FechaLlegada"];
            DateTime fechaSal = (DateTime)Session["FechaSalida"];
            int huespedes = (int)Session["Huespedes"];
            bool conDesayuno = (bool)Session["ConDesayuno"];
            double total = (double)Session["Total"];

            // Traer nombre de la habitación
            BLLHabitacion bllHab = new BLLHabitacion();
            Habitacion hab = bllHab.ObtenerPorId(idHabitacion);

            // Crear reserva
            BLLReserva bllreserva = new BLLReserva();
            Reserva reserva = new Reserva();
            reserva.Hab = new Habitacion();
            reserva.Usuario_ = new Usuario();

            reserva.Hab.Id_Habitacion = idHabitacion;
            reserva.Usuario_.Id_Usuario = ((Usuario)Session["usuario"]).Id_Usuario;
            reserva.FechaLlegada = fechaLleg;
            reserva.FechaSalida = fechaSal;
            reserva.Huespedes = huespedes;
            reserva.IncluyeDesayuno = conDesayuno;
            reserva.Total = total;
            reserva.Estado = "Confirmada";

            bllreserva.CrearReserva(reserva);

            // Guardar para ReservaExitosa ANTES de remover
            Session["UltimaReserva_Nombre"] = hab.Nombre;
            Session["UltimaReserva_Llegada"] = fechaLleg;
            Session["UltimaReserva_Salida"] = fechaSal;
            Session["UltimaReserva_Total"] = total;

            // Remover sesión del carrito
            Session.Remove("Id_Habitacion");
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