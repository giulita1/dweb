using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Habitacion:IVerificable
    {
        public int Id_Habitacion { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public double PrecioPorNoche { get; set; }
        public int Huespedes { get; set; }
        public string ImagenUrl { get; set; }

        public int DVH { get; set; }
        public string GenerarCadenaDVH()
        {
            return $"{Id_Habitacion}{Nombre}{Tipo}{Descripcion}{PrecioPorNoche}{Huespedes}{ImagenUrl}";
        }
    }
}
