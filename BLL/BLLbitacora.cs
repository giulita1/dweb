
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desarrolloweb.BLL
{
    public class BLLbitacora
    {
        private desarrolloweb.DAL.DALbitacora dalBitacora = new desarrolloweb.DAL.DALbitacora();
        public int InsertarBitacora(int idUsuario, string actividad, string modulo, string criticidad)
        {
            BE.Bitacora bitacora = new BE.Bitacora
            {
                Id_Usuario = idUsuario,
                Actividad = actividad,
                Criticidad = criticidad
            };

            return dalBitacora.insertarbitacora(bitacora);
        }

        public DataTable ListarBitacora()
        {
            try
            {
                DataTable dt = dalBitacora.ListarBitacora();

                if (dt == null || dt.Rows.Count == 0)
                {
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar la lista de bitácora: " + ex.Message);
            }
        }

        public DataTable FiltrarBitacora(string login, string modulo, string evento, string criticidad, DateTime desde, DateTime hasta)
        {
            if (desde > hasta)
            {
                throw new Exception("La fecha de inicio 'Desde' no puede ser mayor que la fecha de fin 'Hasta'.");
            }
            return dalBitacora.FiltrarBitacora(login, modulo, evento, criticidad, desde, hasta);
        }
    }
}