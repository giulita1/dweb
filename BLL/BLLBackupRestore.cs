using desarrolloweb.BLL;
using SEG.singleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBackupRestore
    {
        private DAL.DALBackupRestore dalBackupRestore = new DAL.DALBackupRestore();
        private BLLbitacora bllBitacora = new BLLbitacora();
        private readonly string carpetaPuente = @"C:\Users\Public\Documents";
        int usu = SingletonSession.Instancia.Usuario != null ? SingletonSession.Instancia.Usuario.Cod_Usuario : 0;

        public void BackupDatabase(string carpetaDestinoProyecto)
        {
            if (string.IsNullOrEmpty(carpetaDestinoProyecto) || !Directory.Exists(carpetaDestinoProyecto))
            {
                throw new ArgumentException("La ruta del directorio de destino no es válida.");
            }
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupFileName = $"Backup_Hotel_{timestamp}.bak";
            string rutaPuenteCompleta = Path.Combine(carpetaPuente, backupFileName);
            string rutaDestinoCompleta = Path.Combine(carpetaDestinoProyecto, backupFileName);
            try
            {
                dalBackupRestore.BackupDatabase(rutaPuenteCompleta);
                if (!File.Exists(rutaPuenteCompleta))
                {
                    throw new Exception($"SQL Server no pudo generar el archivo en la ruta puente ({rutaPuenteCompleta}). Es probable que el servicio NT SERVICE\\MSSQL$SQLEXPRESS no tenga permisos en esa carpeta.");
                }
                File.Move(rutaPuenteCompleta, rutaDestinoCompleta);
                bllBitacora.InsertarBitacora(0, "Respaldo de base de datos realizado exitosamente.", "Seguridad", "Alta");
            }
            catch (Exception ex)
            {
                if (File.Exists(rutaPuenteCompleta))
                {
                    File.Delete(rutaPuenteCompleta);
                }
                bllBitacora.InsertarBitacora(0, $"Error al realizar el respaldo de la base de datos: {ex.Message}", "Seguridad", "Alta");
                throw;
            }
        }
        public void RestoreDatabase(string rutaArchivoTemporalWeb)
        {
            if (string.IsNullOrEmpty(rutaArchivoTemporalWeb) || !File.Exists(rutaArchivoTemporalWeb))
            {
                throw new ArgumentException("La ruta del archivo de respaldo no es válida o el archivo no existe.");
            }
            string fileName = Path.GetFileName(rutaArchivoTemporalWeb);
            string rutaPuenteCompleta = Path.Combine(carpetaPuente, fileName);
            try
            {
                File.Copy(rutaArchivoTemporalWeb, rutaPuenteCompleta, true);
                dalBackupRestore.RestoreDatabase(rutaPuenteCompleta);
                bllBitacora.InsertarBitacora(usu, $"Restauración de base de datos realizada exitosamente por {usu}.", "Seguridad", "Alta");
            }
            catch (Exception ex)
            {
                bllBitacora.InsertarBitacora(usu, $"Error al restaurar la base de datos: {ex.Message}", "Seguridad", "Alta");
                throw;
            }
            finally
            {
                if (File.Exists(rutaPuenteCompleta))
                {
                    File.Delete(rutaPuenteCompleta);
                }
            }
        }
    }
}
