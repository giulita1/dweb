using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace desarrolloweb.BE
{
    public class Usuario
    {

        public int Cod_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Apellido { get; set; }
        public string User { get; set; }
        public string Contrasena { get; set; }
        public bool Bloqueado { get; set; }
        public int Intentos { get; set; }

    }
}