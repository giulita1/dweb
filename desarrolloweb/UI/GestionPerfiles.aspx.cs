using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class GestionPerfiles : System.Web.UI.Page
    {
        BLLPerfil bll = new BLLPerfil();
        protected void Page_Load(object sender, EventArgs e)
        {
            var usuario = Session["usuario"] as BE.Usuario;
            if (usuario == null)
            {
                Response.Redirect("~/UI/Login.aspx", true);
                return;
            }

            if (usuario.Perfil?.TienePermiso("GestionPerfiles") != true)
            {
                Response.Redirect("~/UI/Inicio.aspx", true);
                return;
            }

            if (!IsPostBack)                
                CargarTodo();
        }

        private void CargarTodo()
        {
            CargarPerfiles();
            CargarFamilias();
            CargarPatentes();
        }

        private void CargarPerfiles()
        {
            rptPerfiles.DataSource = bll.ObtenerTodos();
            rptPerfiles.DataBind();
        }

        private void CargarFamilias()
        {
            List<Familia> familias = bll.ObtenerFamilias();
            rptFamilias.DataSource = familias;
            rptFamilias.DataBind();

            ddlFamiliasParaPerfil.DataSource = familias;
            ddlFamiliasParaPerfil.DataTextField = "Nombre";
            ddlFamiliasParaPerfil.DataValueField = "Id";
            ddlFamiliasParaPerfil.DataBind();

            ddlSubfamilias.DataSource = familias;
            ddlSubfamilias.DataTextField = "Nombre";
            ddlSubfamilias.DataValueField = "Id";
            ddlSubfamilias.DataBind();
        }

        private void CargarPatentes()
        {
            List<Patente> patentes = bll.ObtenerPatentes();
            rptPatentes.DataSource = patentes;
            rptPatentes.DataBind();

            ddlPatentesParaPerfil.DataSource = patentes;
            ddlPatentesParaPerfil.DataTextField = "Nombre";
            ddlPatentesParaPerfil.DataValueField = "Id";
            ddlPatentesParaPerfil.DataBind();

            ddlPatentesParaFamilia.DataSource = patentes;
            ddlPatentesParaFamilia.DataTextField = "Nombre";
            ddlPatentesParaFamilia.DataValueField = "Id";
            ddlPatentesParaFamilia.DataBind();
        }

        // perfiles
        protected void btnAgregarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                bll.AgregarPerfil(new Perfil
                {
                    Nombre = txtNombrePerfil.Text.Trim()
                });
                txtNombrePerfil.Text = "";
                txtDescPerfil.Text = "";
                MostrarMensaje("Perfil agregado correctamente.", "ok");
                CargarPerfiles();
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        protected void rptPerfiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "EliminarPerfil":
                    bll.EliminarPerfil(id);
                    MostrarMensaje("Perfil eliminado.", "ok");
                    CargarPerfiles();
                    break;

                case "AbrirAgregarFamilia":
                    hfIdPerfilSeleccionado.Value = id.ToString();
                    lblNombrePerfilSeleccionado.Text = ObtenerNombrePerfil(id);
                    pnlAgregarFamiliaAPerfil.Visible = true;
                    pnlAgregarPatenteAPerfil.Visible = false;
                    pnlArbol.Visible = false;
                    CargarFamilias();
                    break;

                case "AbrirAgregarPatente":
                    hfIdPerfilSeleccionado2.Value = id.ToString();
                    lblNombrePerfilSeleccionado2.Text = ObtenerNombrePerfil(id);
                    pnlAgregarPatenteAPerfil.Visible = true;
                    pnlAgregarFamiliaAPerfil.Visible = false;
                    pnlArbol.Visible = false;
                    CargarPatentes();
                    break;

                case "VerArbol":
                    Perfil perfil = bll.CargarArbol(id);
                    lblNombreArbol.Text = perfil?.Nombre;
                    litArbol.Text = RenderizarArbol(perfil);
                    pnlArbol.Visible = true;
                    pnlAgregarFamiliaAPerfil.Visible = false;
                    pnlAgregarPatenteAPerfil.Visible = false;
                    break;
            }
        }

        protected void btnConfirmarFamiliaAPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                int idPerfil = Convert.ToInt32(hfIdPerfilSeleccionado.Value);
                int idFamilia = Convert.ToInt32(ddlFamiliasParaPerfil.SelectedValue);
                bll.AgregarFamiliaAPerfil(idPerfil, idFamilia);
                pnlAgregarFamiliaAPerfil.Visible = false;
                MostrarMensaje("Familia agregada al perfil.", "ok");
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        protected void btnConfirmarPatenteAPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                int idPerfil = Convert.ToInt32(hfIdPerfilSeleccionado2.Value);
                int idPatente = Convert.ToInt32(ddlPatentesParaPerfil.SelectedValue);
                bll.AgregarPatenteAPerfil(idPerfil, idPatente);
                pnlAgregarPatenteAPerfil.Visible = false;
                MostrarMensaje("Patente agregada al perfil.", "ok");
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        // familias
        protected void btnAgregarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                bll.AgregarFamilia(new Familia
                {
                    Nombre = txtNombreFamilia.Text.Trim(),
                   
                });
                txtNombreFamilia.Text = "";
                txtDescFamilia.Text = "";
                MostrarMensaje("Familia agregada correctamente.", "ok");
                CargarFamilias();
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        protected void rptFamilias_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "EliminarFamilia":
                    bll.EliminarFamilia(id);
                    MostrarMensaje("Familia eliminada.", "ok");
                    CargarFamilias();
                    break;

                case "AbrirAgregarSubfamilia":
                    hfIdFamiliaSeleccionada.Value = id.ToString();
                    lblNombreFamiliaSeleccionada.Text = ObtenerNombreFamilia(id);
                    pnlAgregarSubfamilia.Visible = true;
                    pnlAgregarPatenteAFamilia.Visible = false;
                    CargarFamilias();
                    break;

                case "AbrirAgregarPatenteAFamilia":
                    hfIdFamiliaSeleccionada2.Value = id.ToString();
                    lblNombreFamiliaSeleccionada2.Text = ObtenerNombreFamilia(id);
                    pnlAgregarPatenteAFamilia.Visible = true;
                    pnlAgregarSubfamilia.Visible = false;
                    CargarPatentes();
                    break;
            }
        }

        protected void btnConfirmarSubfamilia_Click(object sender, EventArgs e)
        {
            try
            {
                int idPadre = Convert.ToInt32(hfIdFamiliaSeleccionada.Value);
                int idHijo = Convert.ToInt32(ddlSubfamilias.SelectedValue);
                if (idPadre == idHijo) throw new Exception("Una familia no puede contenerse a sí misma.");
                bll.AgregarFamiliaAFamilia(idPadre, idHijo);
                pnlAgregarSubfamilia.Visible = false;
                MostrarMensaje("Subfamilia agregada.", "ok");
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        protected void btnConfirmarPatenteAFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                int idFamilia = Convert.ToInt32(hfIdFamiliaSeleccionada2.Value);
                int idPatente = Convert.ToInt32(ddlPatentesParaFamilia.SelectedValue);
                bll.AgregarPatenteAFamilia(idFamilia, idPatente);
                pnlAgregarPatenteAFamilia.Visible = false;
                MostrarMensaje("Patente agregada a la familia.", "ok");
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        // patentes
        protected void btnAgregarPatente_Click(object sender, EventArgs e)
        {
            try
            {
                bll.AgregarPatente(new Patente
                {
                    Nombre = txtNombrePatente.Text.Trim()
                });
                txtNombrePatente.Text = "";
                txtDescPatente.Text = "";
                MostrarMensaje("Patente agregada correctamente.", "ok");
                CargarPatentes();
            }
            catch (Exception ex) { MostrarMensaje(ex.Message, "error"); }
        }

        protected void rptPatentes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EliminarPatente")
            {
                bll.EliminarPatente(Convert.ToInt32(e.CommandArgument));
                MostrarMensaje("Patente eliminada.", "ok");
                CargarPatentes();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAgregarFamiliaAPerfil.Visible = false;
            pnlAgregarPatenteAPerfil.Visible = false;
            pnlAgregarSubfamilia.Visible = false;
            pnlAgregarPatenteAFamilia.Visible = false;
            pnlArbol.Visible = false;
        }

        private string RenderizarArbol(IComponente componente, int nivel = 0)
        {
            if (componente == null) return "";
            StringBuilder sb = new StringBuilder();
            string indent = new string(' ', nivel * 4);
            string icono = componente is Patente ? "key" : componente is Familia ? "folder" : "person";

            sb.Append($"<div class='arbol-nodo nivel-{nivel}'>");
            sb.Append($"<span class='material-symbols-outlined arbol-icono'>{icono}</span>");
            sb.Append($"<span class='arbol-nombre'>{componente.Nombre}</span>");
            sb.Append($"<span class='arbol-tipo'>{componente.GetType().Name}</span>");
            sb.Append("</div>");

            if (componente is Familia fam && fam.Hijos != null)
                foreach (IComponente hijo in fam.Hijos)
                    sb.Append(RenderizarArbol(hijo, nivel + 1));
            else if (componente is Perfil perf && perf.Hijos != null)
                foreach (IComponente hijo in perf.Hijos)
                    sb.Append(RenderizarArbol(hijo, nivel + 1));

            return sb.ToString();
        }

        private string ObtenerNombrePerfil(int id)
        {
            var p = bll.ObtenerTodos().Find(x => x.Id == id);
            return p?.Nombre ?? "";
        }

        private string ObtenerNombreFamilia(int id)
        {
            var f = bll.ObtenerFamilias().Find(x => x.Id == id);
            return f?.Nombre ?? "";
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = $"lbl-mensaje {tipo}";
            lblMensaje.Visible = true;
        }

    }
}