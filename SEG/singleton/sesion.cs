using desarrolloweb.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.singleton
{
    public class sesion
    {
        IUsuario _usuario;

        public IUsuario Usuario
        {
            get { return _usuario; }
        }

        public void IniciarSesion(IUsuario usuario)
        {
            _usuario = usuario;
        }

        public void CerrarSesion()
        {
            _usuario = null;
        }

        public bool EstaAutenticado()
        {
            return _usuario != null;
        }
    }
}
