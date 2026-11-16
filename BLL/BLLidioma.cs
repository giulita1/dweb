using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BLLidioma
    {
        private readonly DALidioma repoidioma = new DALidioma();

        public string InsertarIdioma(Idiomas id)
        {
            if (id == null || string.IsNullOrWhiteSpace(id.Nombre))
                throw new ArgumentException("El nombre del idioma es obligatorio.");

            return repoidioma.InsertarIdioma(id);
        }

        public int ObtenerTraduccion(string n)
        {
            return repoidioma.ObtenerTraduccion(n);
        }

        public string EditarTraduccion(string t, string n, int i)
        {
            if (string.IsNullOrWhiteSpace(n))
                throw new ArgumentException("El nombre de la etiqueta es obligatorio.");

            return repoidioma.EditarTraduccion(t, n, i);
        }

        public List<Etiquetas> ObtenerEyT(string nombre)
        {
            return repoidioma.ObtenerEtiquetasYTraduccion(nombre);
        }

        public string InsertarEtiquetas(int idI, int idE, string t)
        {
            return repoidioma.InsertarE(idI, idE, t);
        }

        public List<Etiqueta> GetAllEtiquetas()
        {
            return repoidioma.GetAllEtiquetas();
        }

        public int CrearIdiomaConDuplicados(string nombreIdioma)
        {
            if (string.IsNullOrWhiteSpace(nombreIdioma))
                throw new ArgumentException("Debe ingresar un nombre para el nuevo idioma.");

            int nuevoIdiomaId = repoidioma.AgregarNuevoIdioma(nombreIdioma.Trim());
            if (nuevoIdiomaId > 0)
            {
                repoidioma.DuplicarRegistrosParaNuevoIdioma(nuevoIdiomaId);
            }

            return nuevoIdiomaId;
        }
    }
}