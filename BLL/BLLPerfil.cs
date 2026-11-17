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

       

        // patentes
        public List<Patente> ObtenerPatentes() => dal.ObtenerPatentes();

        public void AgregarPatente(Patente patente)
        {
            if (string.IsNullOrWhiteSpace(patente.Nombre))
                throw new ArgumentException("El nombre de la patente es obligatorio.");
            dal.AgregarPatente(patente);
        }

        public void AgregarPatenteAFamilia(int idFamilia, int idPatente)
        {
            var familias = new HashSet<int>();
            var patentes = new HashSet<int>();
            RecolectarDeFamilia(idFamilia, familias, patentes);

            if (patentes.Contains(idPatente))
                throw new Exception("No se puede agregar: la patente ya está en esta familia (directamente o dentro de una subfamilia).");

            // ¿algún perfil o familia que contiene a esta familia ya tiene esa patente?
            ValidarContenedoresDeFamilia(idFamilia, new HashSet<int>(), new HashSet<int> { idPatente });

            dal.AgregarPatenteAFamilia(idFamilia, idPatente);
        }

        public void AgregarFamiliaAFamilia(int idPadre, int idHijo)
        {
            if (idPadre == idHijo)
                throw new Exception("No se puede agregar: una familia no puede incluirse a sí misma.");

            var familiasPadre = new HashSet<int>();
            var patentesPadre = new HashSet<int>();
            RecolectarDeFamilia(idPadre, familiasPadre, patentesPadre);

            if (familiasPadre.Contains(idHijo))
                throw new Exception("No se puede agregar: la subfamilia ya está en esta familia (directamente o dentro de otra subfamilia).");

            var familiasHijo = new HashSet<int>();
            var patentesHijo = new HashSet<int>();
            RecolectarDeFamilia(idHijo, familiasHijo, patentesHijo);

            if (familiasHijo.Contains(idPadre))
                throw new Exception("No se puede agregar: esa familia ya contiene a la familia destino y se generaría un ciclo.");

            // Lo que trae la subfamilia no puede repetir nada de la familia destino...
            ValidarSinSolapamiento(familiasPadre, patentesPadre, idHijo, "esta familia");

            // ...ni de ningún perfil o familia que ya contenga a la familia destino
            ValidarContenedoresDeFamilia(idPadre, familiasHijo, patentesHijo);

            dal.AgregarFamiliaAFamilia(idPadre, idHijo);
        }

       
        public void AgregarFamiliaAPerfil(int idPerfil, int idFamilia)
        {
            var familias = new HashSet<int>();
            var patentes = new HashSet<int>();
            RecolectarDePerfil(idPerfil, familias, patentes);

            if (familias.Contains(idFamilia))
                throw new Exception("No se puede agregar: la familia ya está en este perfil (directamente o dentro de otra familia).");

            // La familia nueva no puede traer nada que el perfil ya tenga
            ValidarSinSolapamiento(familias, patentes, idFamilia, "este perfil");

            dal.AgregarFamiliaAPerfil(idPerfil, idFamilia);
        }

       

        // Compara el contenido de la familia nueva contra el del destino y avisa qué se repite
        private void ValidarSinSolapamiento(HashSet<int> famDestino, HashSet<int> patDestino, int idFamiliaNueva, string destino)
        {
            var famNueva = new HashSet<int>();
            var patNueva = new HashSet<int>();
            RecolectarDeFamilia(idFamiliaNueva, famNueva, patNueva);

            var patRepetidas = patNueva.Intersect(patDestino).ToList();
            if (patRepetidas.Count > 0)
            {
                string nombres = string.Join(", ", dal.ObtenerPatentes()
                    .Where(p => patRepetidas.Contains(p.Id)).Select(p => p.Nombre));
                throw new Exception($"No se puede agregar: la familia contiene patente(s) que ya están en {destino}: {nombres}.");
            }

            var famRepetidas = famNueva.Intersect(famDestino).ToList();
            if (famRepetidas.Count > 0)
            {
                string nombres = string.Join(", ", dal.ObtenerFamilias()
                    .Where(f => famRepetidas.Contains(f.Id)).Select(f => f.Nombre));
                throw new Exception($"No se puede agregar: la familia contiene familia(s) que ya están en {destino}: {nombres}.");
            }
        }

        public void AgregarPatenteAPerfil(int idPerfil, int idPatente)
        {
            var familias = new HashSet<int>();
            var patentes = new HashSet<int>();
            RecolectarDePerfil(idPerfil, familias, patentes);

            if (patentes.Contains(idPatente))
                throw new Exception("No se puede agregar: la patente ya está en este perfil (directamente o dentro de una familia).");

            dal.AgregarPatenteAPerfil(idPerfil, idPatente);
        }

        public void EliminarFamiliaDePerfil(int idPerfil, int idFamilia) => dal.EliminarFamiliaDePerfil(idPerfil, idFamilia);
        public void EliminarPatenteDePerfil(int idPerfil, int idPatente) => dal.EliminarPatenteDePerfil(idPerfil, idPatente);


        public void EliminarFamiliaDeFamily(int idPadre, int idHijo) => dal.EliminarFamiliaDeFamily(idPadre, idHijo);
        public void EliminarPatenteDeFamilia(int idFamilia, int idPatente) => dal.EliminarPatenteDeFamilia(idFamilia, idPatente);

        //juntan todo lo que contiene un perfil o una familia
        private void RecolectarDePerfil(int idPerfil, HashSet<int> familias, HashSet<int> patentes)
        {
            foreach (var p in dal.ObtenerPatentesDePerfil(idPerfil))
                patentes.Add(p.Id);

            foreach (var f in dal.ObtenerFamiliasDePerfil(idPerfil))
                RecolectarDeFamilia(f.Id, familias, patentes);
        }

        // Revisa todos los perfiles y familias que contienen a idFamilia:
        // lo que se agrega no puede repetir nada de lo que ya tienen
        private void ValidarContenedoresDeFamilia(int idFamilia, HashSet<int> famNueva, HashSet<int> patNueva)
        {
            var todasFam = dal.ObtenerFamilias();
            var todasPat = dal.ObtenerPatentes();

            foreach (var perfil in dal.ObtenerTodos())
            {
                var fams = new HashSet<int>();
                var pats = new HashSet<int>();
                RecolectarDePerfil(perfil.Id, fams, pats);
                if (!fams.Contains(idFamilia)) continue;
                ValidarInterseccion(fams, pats, famNueva, patNueva, "el perfil " + perfil.Nombre, todasFam, todasPat);
            }

            foreach (var fam in todasFam)
            {
                if (fam.Id == idFamilia) continue; // el destino directo ya se validó aparte
                var fams = new HashSet<int>();
                var pats = new HashSet<int>();
                RecolectarDeFamilia(fam.Id, fams, pats);
                if (!fams.Contains(idFamilia)) continue;
                ValidarInterseccion(fams, pats, famNueva, patNueva, "la familia " + fam.Nombre, todasFam, todasPat);
            }
        }

        private void ValidarInterseccion(HashSet<int> famC, HashSet<int> patC,
            HashSet<int> famNueva, HashSet<int> patNueva, string contenedor,
            List<Familia> todasFam, List<Patente> todasPat)
        {
            var patRepetidas = patNueva.Intersect(patC).ToList();
            if (patRepetidas.Count > 0)
            {
                string nombres = string.Join(", ", todasPat.Where(p => patRepetidas.Contains(p.Id)).Select(p => p.Nombre));
                throw new Exception($"No se puede agregar: generaría una patente duplicada en {contenedor}: {nombres}.");
            }

            var famRepetidas = famNueva.Intersect(famC).ToList();
            if (famRepetidas.Count > 0)
            {
                string nombres = string.Join(", ", todasFam.Where(f => famRepetidas.Contains(f.Id)).Select(f => f.Nombre));
                throw new Exception($"No se puede agregar: generaría una familia duplicada en {contenedor}: {nombres}.");
            }
        }
        private void RecolectarDeFamilia(int idFamilia, HashSet<int> familias, HashSet<int> patentes)
        {
            if (!familias.Add(idFamilia)) return; // ya visitada: también evita bucles infinitos

            foreach (var p in dal.ObtenerPatentesDeFamilia(idFamilia))
                patentes.Add(p.Id);

            foreach (var sub in dal.ObtenerSubfamilias(idFamilia))
                RecolectarDeFamilia(sub.Id, familias, patentes);
        }



        // arbol completo
        public Perfil CargarArbol(int idPerfil) => dal.CargarArbol(idPerfil);

        public Familia CargarArbolFamilia(int idFamilia) => dal.CargarArbolFamilia(idFamilia);

        // verificar permiso
        public bool TienePermiso(int idPerfil, string nombrePatente)
        {
            Perfil perfil = dal.CargarArbol(idPerfil);
            if (perfil == null) return false;
            return perfil.TienePermiso(nombrePatente);
        }

        public void EliminarPerfil(int idPerfil)
        {
            if (idPerfil <= 0)
                throw new ArgumentException("Perfil inválido.");

            int usuarios = dal.ContarUsuariosDePerfil(idPerfil);
            if (usuarios > 0)
                throw new Exception($"No se puede eliminar el perfil: ya está asignado a {usuarios} usuario(s).");

            dal.EliminarPerfil(idPerfil);
        }

        public void EliminarFamilia(int idFamilia)
        {
            if (idFamilia <= 0)
                throw new ArgumentException("Familia inválida.");

            int perfiles = dal.ContarPerfilesDeFamilia(idFamilia);
            int padres = dal.ContarFamiliasPadreDeFamilia(idFamilia);

            if (perfiles > 0)
                throw new Exception($"No se puede eliminar la familia: ya está asignada a {perfiles} perfil(es).");
            if (padres > 0)
                throw new Exception($"No se puede eliminar la familia: ya está incluida en {padres} familia(s).");

            dal.EliminarFamilia(idFamilia);
        }

        public void EliminarPatente(int idPatente)
        {
            if (idPatente <= 0)
                throw new ArgumentException("Patente inválida.");

            int perfiles = dal.ContarPerfilesDePatente(idPatente);
            int familias = dal.ContarFamiliasDePatente(idPatente);

            if (perfiles > 0)
                throw new Exception($"No se puede eliminar la patente: ya está asignada a {perfiles} perfil(es).");
            if (familias > 0)
                throw new Exception($"No se puede eliminar la patente: ya está asignada a {familias} familia(s).");

            dal.EliminarPatente(idPatente);
        }

        // Lecturas para "Quitar"
        public List<Familia> ObtenerFamiliasDePerfil(int idPerfil) => dal.ObtenerFamiliasDePerfil(idPerfil);
        public List<Patente> ObtenerPatentesDePerfil(int idPerfil) => dal.ObtenerPatentesDePerfil(idPerfil);
        public List<Familia> ObtenerSubfamilias(int idFamilia) => dal.ObtenerSubfamilias(idFamilia);
        public List<Patente> ObtenerPatentesDeFamilia(int idFamilia) => dal.ObtenerPatentesDeFamilia(idFamilia);
    }
}

