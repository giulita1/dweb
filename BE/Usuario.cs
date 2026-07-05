using desarrolloweb.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace desarrolloweb.BE
{
    public class Usuario : IUsuario
    {

        public int Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Apellido { get; set; }
        public string User { get; set; }
        public string Contrasena { get; set; }
        public bool Bloqueado { get; set; }
        public int Intentos { get; set; }
        public int DVH { get; set; }
        public int IdRol { get; set; }
        public string GenerarCadenaDVH()
        {
            return $"{Id_Usuario}{Nombre}{Email}{Apellido}{User}{Contrasena}{Bloqueado}{Intentos}";
        }
    }
}