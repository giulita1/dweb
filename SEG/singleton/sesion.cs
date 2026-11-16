using BE;
using desarrolloweb.BE;
using SEG.Observer;
using System.Collections.Generic;

namespace SEG.singleton
{
    public class sesion
    {
        private IUsuario _usuario;
        private Iidioma _idiomaActual;
        private static readonly IList<IdiomaObserver> _observers = new List<IdiomaObserver>();

        public IUsuario Usuario
        {
            get { return _usuario; }
        }

        public Iidioma IdiomaActual
        {
            get { return _idiomaActual; }
            set { _idiomaActual = value; }
        }

        public void IniciarSesion(IUsuario usuario)
        {
            _usuario = usuario;
        }

        public void CerrarSesion()
        {
            _usuario = null;
        }

        public bool EstaAutenticado()
        {
            return _usuario != null;
        }

        // ── Métodos del Patrón Observer ──

        public void SuscribirObservador(IdiomaObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void DesuscribirObservador(IdiomaObserver observer)
        {
            if (observer != null && _observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void CambiarIdioma(Iidioma idioma)
        {
            _idiomaActual = idioma;
            NotificarObservadores();
        }

        public void NotificarObservadores()
        {
            // Se recorre una copia para evitar errores si algún observador se modifica al notificar
            foreach (var observer in new List<IdiomaObserver>(_observers))
            {
                observer.UpdateLanguage(_idiomaActual);
            }
        }
    }
}