using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IComponente
    {
        int Id { get; }
        string Nombre { get; }
        bool TienePermiso(string nombrePatente);
    }

}
