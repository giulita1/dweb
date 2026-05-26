using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desarrolloweb.BE
{
    public class Bitacora
    {
        public int Id_Bitacora { get; set; }
        public int Id_Usuario { get; set; }
        public string Actividad { get; set; }
        public string Criticidad { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string DVH { get; set; }
    }
}