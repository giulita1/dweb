using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace desarrolloweb.BE
{
    public class Evento
    {
        public int Cod_Evento { get; set; }
        public Usuario Usuario { get; set; }
        public int Criticidad { get; set; }
        public string Nombre { get; set; }
        public string Modulo { get; set; }
    }
}