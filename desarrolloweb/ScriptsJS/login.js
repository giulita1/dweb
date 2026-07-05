function toggleRecuperacion(e) {
    e.preventDefault();
    var panel = document.getElementById('panel-recuperar');
    if (panel.style.display === 'none' || panel.style.display === '') {
        panel.style.display = 'flex';
    } else {
        panel.style.display = 'none';
    }
}