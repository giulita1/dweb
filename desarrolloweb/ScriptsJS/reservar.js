// ── Contador huéspedes ──────────────────────────────────
const btnMenos = document.getElementById('btn-menos-r');
const btnMas = document.getElementById('btn-mas-r');
const display = document.getElementById('display-huespedes-r');
const inputH = document.getElementById('cant-huespedes-r');

if (btnMenos && btnMas) {
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
}

// ── Buscar ──────────────────────────────────────────────
function buscar() {
    const llegada = document.getElementById('fechaLlegada').value;
    const salida = document.getElementById('fechaSalida').value;
    const huespedes = document.getElementById('cant-huespedes-r').value;

    if (!llegada || !salida) {
        alert('Por favor ingresá las fechas de llegada y salida.');
        return;
    }

    if (llegada >= salida) {
        alert('La fecha de salida debe ser posterior a la de llegada.');
        return;
    }

    window.location.href = `Reservar.aspx?llegada=${llegada}&salida=${salida}&huespedes=${huespedes}`;
}

// ── Seleccionar habitación → carrito ────────────────────
function seleccionarHabitacion(id, nombre, tipo, precio, img) {
    const llegada = document.getElementById('fechaLlegada').value;
    const salida = document.getElementById('fechaSalida').value;
    const huespedes = parseInt(document.getElementById('cant-huespedes-r').value);

    const d1 = new Date(llegada);
    const d2 = new Date(salida);
    const noches = Math.round((d2 - d1) / (1000 * 60 * 60 * 24));

    sessionStorage.setItem("reserva", JSON.stringify({
        habitacionId: id,
        nombre: nombre,
        tipo: tipo,
        precioPorNoche: parseFloat(precio),
        imagenUrl: img,
        llegada: d1.toLocaleDateString("es-AR"),
        salida: d2.toLocaleDateString("es-AR"),
        noches: noches,
        huespedes: huespedes
    }));

    // Actualizar carrito y abrirlo
    cargarCarrito();
    abrirCarrito();
}