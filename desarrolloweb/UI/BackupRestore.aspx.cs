using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace desarrolloweb.UI
{
    public partial class BackupRestore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {  if (Session["usuario"] == null)
                {
                    Response.Redirect("~/UI/Login.aspx");
                    return;
                }

            if (!IsPostBack)
            {
                CargarListaBackups();
            }

            
            pnlMensaje.Visible = false;
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            try 
            {
                BLLBackupRestore bllBackup = new BLLBackupRestore();

                string backupDirectory = Server.MapPath("~/Backups/");
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }
                bllBackup.BackupDatabase(backupDirectory);
               MostrarMensaje("Backup realizado con éxito en el servidor.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al realizar el backup: " + ex.Message, false);
            }

        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                string archivoSeleccionado = ddlBackups.SelectedValue;

                if (string.IsNullOrEmpty(archivoSeleccionado))
                {
                    MostrarMensaje("Debe seleccionar un archivo válido para restaurar.", false);
                    return;
                }

                string backupDirectory = Server.MapPath("~/Backups/");
                string filePath = Path.Combine(backupDirectory, archivoSeleccionado);

                BLLBackupRestore bllBackup = new BLLBackupRestore();
                bllBackup.RestoreDatabase(filePath);

                MostrarMensaje("Restauración de la base de datos realizada con éxito.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error en Restore: " + ex.Message, false);
            }

        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
        pnlMensaje.Visible = true;
            lblMensaje.Text = mensaje;
            
            if (esExito)
            {
                pnlMensaje.CssClass = "alert alert-success";
            }
            else
            {
                pnlMensaje.CssClass = "alert alert-danger";
            }
        }
        private void CargarListaBackups()
        {
            string backupDirectory = Server.MapPath("~/Backups/");
            ddlBackups.Items.Clear();

            if (Directory.Exists(backupDirectory))
            {
                var archivos = new DirectoryInfo(backupDirectory)
                                .GetFiles("*.bak")
                                .OrderByDescending(f => f.CreationTime)
                                .ToList();

                if (archivos.Count > 0)
                {
                    foreach (var archivo in archivos)
                    {
                        ddlBackups.Items.Add(new ListItem(archivo.Name, archivo.Name));
                    }
                }
                else
                {
                    ddlBackups.Items.Add(new ListItem("-- No hay backups en el servidor --", ""));
                }
            }
            else
            {
                ddlBackups.Items.Add(new ListItem("-- No hay backups en el servidor --", ""));
            }
        }
    }
}