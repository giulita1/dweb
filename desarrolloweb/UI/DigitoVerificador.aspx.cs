using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace desarrolloweb
{
    public partial class DigitoVerificador : System.Web.UI.Page
    {
        private BLLDVV gestorIntegridad = new BLLDVV();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/UI/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                ValidarIntegridadBaseDatos();
            }
        }

        private void ValidarIntegridadBaseDatos()
        {
            try
            {
                List<Infraccion> errores = gestorIntegridad.VerificarIntegridadGlobal();

                if (errores != null && errores.Count > 0)
                {
                    dgvInfracciones.DataSource = errores;
                    dgvInfracciones.DataBind();
                    dgvInfracciones.Visible = true;
                    pnlMensajeOK.Visible = false;
                }
                else
                {
                    dgvInfracciones.Visible = false;
                    pnlMensajeOK.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MostrarAlertaJS("Error al validar integridad: " + ex.Message);
            }
        }
        protected void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                gestorIntegridad.RecalcularTodosLosDigitos();

                MostrarAlertaJS("Recálculo masivo completado exitosamente. Se ha actualizado la base de datos.");

                ValidarIntegridadBaseDatos();
            }
            catch (Exception ex)
            {
                MostrarAlertaJS(ex.Message);
            }

        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            if (!fuBackup.HasFile)
            {
                MostrarAlertaJS("Debe seleccionar un archivo válido con extensión .bak");
                return;
            }

            try
            {
                string extension = Path.GetExtension(fuBackup.FileName).ToLower();
                if (extension != ".bak")
                {
                    MostrarAlertaJS("El archivo seleccionado no es un respaldo válido (.bak).");
                    return;
                }
                string carpetaTemporal = Server.MapPath("~/App_Data/TempRestore/");
                if (!Directory.Exists(carpetaTemporal))
                {
                    Directory.CreateDirectory(carpetaTemporal);
                }

                string rutaDestinoFisica = Path.Combine(carpetaTemporal, fuBackup.FileName);

                fuBackup.SaveAs(rutaDestinoFisica);

                BLLBackupRestore gestorRestore = new BLLBackupRestore();
                gestorRestore.RestoreDatabase(rutaDestinoFisica);
                if (File.Exists(rutaDestinoFisica))
                {
                    File.Delete(rutaDestinoFisica);
                }

                MostrarAlertaJS("Base de datos restaurada correctamente. El sistema se ha actualizado.");
                ValidarIntegridadBaseDatos();
            }
            catch (Exception ex)
            {
                MostrarAlertaJS("Error durante la restauración del sistema: " + ex.Message);
            }

        }
        private void MostrarAlertaJS(string mensaje)
        {
            string mensajeSeguro = mensaje.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            ClientScript.RegisterStartupScript(this.GetType(), "AlertaIntegridad", $"alert('{mensajeSeguro}');", true);
        }
    }
}