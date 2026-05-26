﻿function limpiarFiltros() {
    const campos = document.querySelectorAll('.js-filter-field');
    campos.forEach(campo => {
        if (campo.tagName === 'INPUT') {
            campo.value = '';
        } else if (campo.tagName === 'SELECT') {
            campo.selectedIndex = 0;
        }
    });
}
function validarFechas(idFechaDesde, idFechaHasta) {
    const fechaDesdeStr = document.getElementById(idFechaDesde).value;
    const fechaHastaStr = document.getElementById(idFechaHasta).value;

    if (fechaDesdeStr && fechaHastaStr) {
        const desde = new Date(fechaDesdeStr);
        const hasta = new Date(fechaHastaStr);

        if (desde > hasta) {
            alert('La "Fecha Desde" no puede ser mayor que la "Fecha Hasta".');
            return false;
        }
    }
    return true;
}