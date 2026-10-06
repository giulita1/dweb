using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Perfil : IComponente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<IComponente> Hijos { get; set; } = new List<IComponente>();

        public void AgregarHijo(IComponente hijo) => Hijos.Add(hijo);
        public void EliminarHijo(IComponente hijo) => Hijos.Remove(hijo);

        public bool TienePermiso(string nombrePatente)
        {
            foreach (IComponente hijo in Hijos)
                if (hijo.TienePermiso(nombrePatente))
                    return true;
            return false;
        }
    }
}
