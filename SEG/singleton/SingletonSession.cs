using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.singleton
{
    public class SingletonSession
    {
        private static sesion _instancia;

        private static object _lock = new object();

        public static sesion Instancia
        {
            get
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new sesion();
                    }
                }
                return _instancia;
            }

        }
    }
}
