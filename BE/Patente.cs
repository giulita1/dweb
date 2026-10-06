using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Patente : IComponente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public bool TienePermiso(string nombrePatente)
        {
            return Nombre.Equals(nombrePatente,
                   System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
