using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using desarrolloweb.BE;

namespace SEG
{
    public class DigitoVerificador
    {
        public int ConvertToAscii(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;

            int asciiValueSum = 0;
            foreach (char c in input)
            {
                asciiValueSum += (int)c;
            }

            return asciiValueSum;
        }
        public int CalcularDVH(IVerificable entidad)
        {
            string cadenaFila = entidad.GenerarCadenaDVH();
            return ConvertToAscii(cadenaFila);
        }
        public int CalcularDVV(List<int> todosLosDvh)
        {
            int dvv = 0;
            foreach (int dvh in todosLosDvh)
            {
                dvv += dvh;
            }
            return dvv;
        }

    }
}
