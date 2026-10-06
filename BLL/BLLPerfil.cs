using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPerfil
    {
        private DALPerfil dal = new DALPerfil();

        //perfiles

        public void AgregarPerfil(Perfil perfil)
        {
            if (string.IsNullOrWhiteSpace(perfil.Nombre))
                throw new ArgumentException("El nombre del perfil es obligatorio.");
            dal.AgregarPerfil(perfil);
        }

        public void EliminarPerfil(int idPerfil)
        {
            if (idPerfil <= 0)
                throw new ArgumentException("Perfil inválido.");
            dal.EliminarPerfil(idPerfil);
        }

        public List<Perfil> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }


        //familias
        public List<Familia> ObtenerFamilias() => dal.ObtenerFamilias();

        public void AgregarFamilia(Familia familia)
        {
            if (string.IsNullOrWhiteSpace(familia.Nombre))
                throw new ArgumentException("El nombre de la familia es obligatorio.");
            dal.AgregarFamilia(familia);
        }

        public void EliminarFamilia(int idFamilia)
        {
            if (idFamilia <= 0)
                throw new ArgumentException("Familia inválida.");
            dal.EliminarFamilia(idFamilia);
        }

        // patentes
        public List<Patente> ObtenerPatentes() => dal.ObtenerPatentes();

        public void AgregarPatente(Patente patente)
        {
            if (string.IsNullOrWhiteSpace(patente.Nombre))
                throw new ArgumentException("El nombre de la patente es obligatorio.");
            dal.AgregarPatente(patente);
        }

        public void EliminarPatente(int idPatente)
        {
            if (idPatente <= 0)
                throw new ArgumentException("Patente inválida.");
            dal.EliminarPatente(idPatente);
        }

        //relaciones perfil
        public void AgregarFamiliaAPerfil(int idPerfil, int idFamilia) => dal.AgregarFamiliaAPerfil(idPerfil, idFamilia);
        public void EliminarFamiliaDePerfil(int idPerfil, int idFamilia) => dal.EliminarFamiliaDePerfil(idPerfil, idFamilia);
        public void AgregarPatenteAPerfil(int idPerfil, int idPatente) => dal.AgregarPatenteAPerfil(idPerfil, idPatente);
        public void EliminarPatenteDePerfil(int idPerfil, int idPatente) => dal.EliminarPatenteDePerfil(idPerfil, idPatente);

        //relaciones familia
        public void AgregarFamiliaAFamilia(int idPadre, int idHijo) => dal.AgregarFamiliaAFamilia(idPadre, idHijo);
        public void EliminarFamiliaDeFamily(int idPadre, int idHijo) => dal.EliminarFamiliaDeFamily(idPadre, idHijo);
        public void AgregarPatenteAFamilia(int idFamilia, int idPatente) => dal.AgregarPatenteAFamilia(idFamilia, idPatente);
        public void EliminarPatenteDeFamilia(int idFamilia, int idPatente) => dal.EliminarPatenteDeFamilia(idFamilia, idPatente);

        // arbol completo
        public Perfil CargarArbol(int idPerfil) => dal.CargarArbol(idPerfil);

        // verificar permiso
        public bool TienePermiso(int idPerfil, string nombrePatente)
        {
            Perfil perfil = dal.CargarArbol(idPerfil);
            if (perfil == null) return false;
            return perfil.TienePermiso(nombrePatente);
        }
    }
}

