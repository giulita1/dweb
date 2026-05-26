using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desarrolloweb.BE
{
    public class Bitacora
    {
        private int idbitacora;
        public int IdBitacora
        {
            get { return idbitacora; }
            set { idbitacora = value; }
        }

        private string usu;
        public string Usu
        {
            get { return usu; }
            set { usu = value; }
        }

        private DateTime fechacambio;
        public DateTime FechaCambio
        {
            get { return fechacambio; }
            set { fechacambio = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private string modulo;
        public string Modulo
        {
            get { return modulo; }
            set { modulo = value; }
        }

        private string criticidad;
        public string Criticidad
        {
            get { return criticidad; }
            set { criticidad = value; }
        }

    }
}