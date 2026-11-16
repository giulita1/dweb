using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Idiomas : Iidioma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Default { get; set; }
        public Etiquetas Etiquetas { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
