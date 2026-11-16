using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class IdiomaModelo : Iidioma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Default { get; set; }
        public Etiquetas Etiquetas { get; set; }
        public IEtiqueta IEtiqueta { get; set; }

        public IdiomaModelo(int id)
        {
            Id = id;
            Nombre = string.Empty;
            Default = false;
        }

        public IdiomaModelo()
        {
        }
    }
}
