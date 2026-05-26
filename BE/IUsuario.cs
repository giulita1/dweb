using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desarrolloweb.BE
{
    public interface IUsuario
    {
        int Cod_Usuario { get; set; }
        string User { get; set; }
        string Contrasena { get; set; }
    }
}
