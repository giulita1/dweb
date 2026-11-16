using BE;
using BLL;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class GestionIdioma : System.Web.UI.Page
    {
        private readonly BLLidioma gestoridioma = new BLLidioma();
        private readonly BLLTraductor tradu = new BLLTraductor();

        protected void Page_Load(object sender, EventArgs e)
        {
            var usuActual = Session["usuario"] as desarrolloweb.BE.Usuario;
            if (usuActual == null || usuActual.Perfil?.TienePermiso("CambiarIdioma") != true)
            {
                Response.Redirect("~/UI/Inicio.aspx", true);
                return;
            }

            if (!IsPostBack)
            {
                ObtenerIdiomas();
                if (ddlIdiomas.Items.Count > 0)
                {
                    EnlazarGrid();
                }
            }
        }

        public void ObtenerIdiomas()
        {
            ddlIdiomas.DataSource = tradu.ObtenerIdiomas();
            ddlIdiomas.DataTextField = "Nombre";
            ddlIdiomas.DataValueField = "Id";
            ddlIdiomas.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreIdioma = txtIdioma.Text.Trim();
                if (string.IsNullOrEmpty(nombreIdioma))
                {
                    MostrarAlerta("Ingrese un nombre para el nuevo idioma.");
                    return;
                }

                int nuevoId = gestoridioma.CrearIdiomaConDuplicados(nombreIdioma);

                if (nuevoId > 0)
                {
                    txtIdioma.Text = string.Empty;
                    ObtenerIdiomas();
                    ddlIdiomas.SelectedValue = nuevoId.ToString();
                    EnlazarGrid();
                    MostrarAlerta("Idioma creado y etiquetas duplicadas correctamente.");
                }
                else
                {
                    MostrarAlerta("No se pudo crear el idioma.");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error: " + ex.Message);
            }
        }

        protected void ddlIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnlazarGrid();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            EnlazarGrid();
        }

        public void EnlazarGrid()
        {
            if (ddlIdiomas.SelectedItem != null)
            {
                gvTraducciones.DataSource = gestoridioma.ObtenerEyT(ddlIdiomas.SelectedItem.Text);
                gvTraducciones.DataBind();
            }
        }

        protected void gvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTraducciones.Rows[rowIndex];

                txtEtiqueta.Text = HttpUtility.HtmlDecode(row.Cells[1].Text);
                txtTraduccion.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlIdiomas.SelectedItem == null)
                {
                    MostrarAlerta("Seleccione un idioma.");
                    return;
                }

                int idiomaId = Convert.ToInt32(ddlIdiomas.SelectedValue);
                string etiqueta = txtEtiqueta.Text.Trim();
                string traduccion = txtTraduccion.Text.Trim();

                if (idiomaId > 0 && !string.IsNullOrEmpty(etiqueta) && !string.IsNullOrEmpty(traduccion))
                {
                    string mensaje = gestoridioma.EditarTraduccion(traduccion, etiqueta, idiomaId);
                    EnlazarGrid();
                    MostrarAlerta(mensaje);
                }
                else
                {
                    MostrarAlerta("Seleccione una etiqueta de la tabla y complete la traducción.");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al guardar: " + ex.Message);
            }
        }

        private void MostrarAlerta(string mensaje)
        {
            string limpio = mensaje.Replace("'", "\\'").Replace("\r\n", " ").Replace("\n", " ");
            ScriptManager.RegisterStartupScript(this, GetType(), "alerta", $"alert('{limpio}');", true);
        }
    }
}