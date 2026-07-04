const btnMenos = document.getElementById('btn-menos');
const btnMas = document.getElementById('btn-mas');
const display = document.getElementById('display-huespedes');
const inputH = document.getElementById('cant-huespedes');

btnMenos.addEventListener('click', () => {
    let val = parseInt(inputH.value);
    if (val > 1) {
        val--;
        inputH.value = val;
        display.textContent = val;
    }
    btnMenos.disabled = val === 1;
    btnMas.disabled = false;
});

btnMas.addEventListener('click', () => {
    let val = parseInt(inputH.value);
    if (val < 4) {
        val++;
        inputH.value = val;
        display.textContent = val;
    }
    btnMas.disabled = val === 4;
    btnMenos.disabled = false;
});

document.querySelector('.btn-reservar').addEventListener('click', () => {
    const llegada = document.getElementById('fecha-llegada').value;
    const salida = document.getElementById('fecha-salida').value;
    const huespedes = inputH.value;

    if (!llegada || !salida) {
        alert('Por favor ingresá las fechas.');
        return;
    }

    if (llegada >= salida) {
        alert('La fecha de salida debe ser posterior a la de llegada.');
        return;
    }

    window.location.href = `/UI/Reservar.aspx?llegada=${llegada}&salida=${salida}&huespedes=${huespedes}`;
});

// ── Botón RESERVAR ──────────────────────────────────────
document.querySelector('.btn-reservar').addEventListener('click', () => {
    const llegada = document.getElementById('fecha-llegada').value;
    const salida = document.getElementById('fecha-salida').value;
    const huespedes = document.getElementById('cant-huespedes').value;

    if (!llegada || !salida) {
        alert('Por favor ingresá las fechas.');
        return;
    }

    if (llegada >= salida) {
        alert('La fecha de salida debe ser posterior a la de llegada.');
        return;
    }

    window.location.href = `/UI/Reservar.aspx?llegada=${llegada}&salida=${salida}&huespedes=${huespedes}`;
});