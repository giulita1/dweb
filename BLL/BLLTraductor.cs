using BE;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class BLLTraductor
    {
        private readonly DALidioma irepo = new DALidioma();

        public Iidioma Idioma { get; set; }

        public Iidioma ObtenerIdiomaDefault()
        {
            return irepo.ObtenerIdiomaDefault();
        }

        public IList<Iidioma> ObtenerIdiomas()
        {
            return irepo.ObtenerIdiomas();
        }

        public IDictionary<string, ITraduccion> ObtenerTraducciones(Iidioma idioma = null)
        {
            return irepo.ObtenerTraducciones(idioma);
        }
    }
}