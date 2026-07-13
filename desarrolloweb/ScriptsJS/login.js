function toggleRecuperacion(event) {
    event.preventDefault();
    var panel = document.getElementById("panel-recuperar");

    panel.classList.toggle("visible");
}

function enviarClave() {
    const email = document.getElementById('txtEmailRecuperar').value;
    if (!email) {
        alert('Por favor ingresá tu email.');
        return;
    }
    alert('Si el email existe en el sistema, recibirás una nueva clave.');
}