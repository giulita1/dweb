function buscar() {
    const busqueda = document.getElementById('txtBusqueda').value;
    const bloqueados = document.getElementById('chkBloqueados').checked ? '1' : '0';
    window.location.href = `GestionUsuarios.aspx?busqueda=${encodeURIComponent(busqueda)}&bloqueados=${bloqueados}`;
}