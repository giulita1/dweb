function confirmarRecalculo() {
    return confirm("¿Está seguro de que desea recalcular masivamente todos los dígitos verificadores? Esta acción actualizará los patrones horizontales y verticales del sistema.");
}

function confirmarRestore() {
    var fileInput = document.getElementById('MainContent_fuBackup');
    if (fileInput && fileInput.files.length === 0) {
        alert("Por favor, seleccione un archivo de respaldo (.bak) antes de proceder.");
        return false;
    }
    return confirm("¡ADVERTENCIA CRÍTICA!\n\nRealizar una restauración reemplazará todos los datos actuales del hotel por los del archivo seleccionado. Esta acción no se puede deshacer.\n\n¿Desea continuar?");
}