// ── Menú mobile ──────────────────────────────────────────
const btnMenu = document.getElementById("btn-menu");
const navMenu = document.getElementById("nav-menu");

if (btnMenu) {
    btnMenu.addEventListener("click", () => {
        navMenu.classList.toggle("active");
    });
}

// ── Carrito ──────────────────────────────────────────────
const btnCarrito = document.getElementById("btn-carrito");
const carrito = document.getElementById("carrito");
const overlay = document.getElementById("overlay-carrito");
const btnCerrar = document.getElementById("btn-cerrar-carrito");
const carritoVacio = document.getElementById("carrito-vacio");
const carritoItem = document.getElementById("carrito-item");
const carritoFooter = document.getElementById("carrito-footer");
const chkDesayuno = document.getElementById("chk-desayuno");
const filaDesayuno = document.getElementById("fila-desayuno");

const PRECIO_DESAYUNO_POR_PERSONA = 5000;

function abrirCarrito() {
    carrito?.classList.add("active");
    overlay?.classList.add("active");
}

function cerrarCarrito() {
    carrito?.classList.remove("active");
    overlay?.classList.remove("active");
}

btnCarrito?.addEventListener("click", abrirCarrito);
btnCerrar?.addEventListener("click", cerrarCarrito);
overlay?.addEventListener("click", cerrarCarrito);

// ── Cargar datos desde sessionStorage ───────────────────
function cargarCarrito() {
    const datos = JSON.parse(sessionStorage.getItem("reserva"));

    if (!datos) return;

    carritoVacio.style.display = "none";
    carritoItem.style.display = "block";
    carritoFooter.style.display = "block";

    document.getElementById("carrito-img").src = datos.imagenUrl;
    document.getElementById("carrito-tipo").textContent = datos.tipo;
    document.getElementById("carrito-nombre").textContent = datos.nombre;
    document.getElementById("carrito-fechas").textContent =
        `${datos.llegada} → ${datos.salida}`;
    document.getElementById("carrito-noches").textContent =
        `${datos.noches} noche${datos.noches > 1 ? "s" : ""}`;

    actualizarTotal(datos);
}

function actualizarTotal(datos) {
    const precioHab = datos.precioPorNoche * datos.noches;
    const huespedes = datos.huespedes;
    const conDesayuno = chkDesayuno?.checked;
    const precioDesay = conDesayuno
        ? PRECIO_DESAYUNO_POR_PERSONA * huespedes * datos.noches
        : 0;
    const total = precioHab + precioDesay;

    const fmt = v => v.toLocaleString("es-AR", { style: "currency", currency: "ARS", maximumFractionDigits: 0 });

    document.getElementById("lbl-precio-hab").textContent =
        `Habitación × ${datos.noches} noche${datos.noches > 1 ? "s" : ""}`;
    document.getElementById("val-precio-hab").textContent = fmt(precioHab);
    document.getElementById("val-total").textContent = fmt(total);

    if (conDesayuno) {
        filaDesayuno.style.display = "flex";
        document.getElementById("lbl-desayuno").textContent =
            `Desayuno × ${huespedes} pax × ${datos.noches} noche${datos.noches > 1 ? "s" : ""}`;
        document.getElementById("val-desayuno").textContent = fmt(precioDesay);
    } else {
        filaDesayuno.style.display = "none";
    }

    // Guardar si quiere desayuno para usarlo al confirmar
    datos.conDesayuno = conDesayuno;
    datos.totalFinal = total;
    sessionStorage.setItem("reserva", JSON.stringify(datos));
}

chkDesayuno?.addEventListener("change", () => {
    const datos = JSON.parse(sessionStorage.getItem("reserva"));
    if (datos) actualizarTotal(datos);
});

document.getElementById("btn-confirmar")?.addEventListener("click", () => {
    const datos = JSON.parse(sessionStorage.getItem("reserva"));
    if (!datos) return;
    window.location.href =
        `/UI/ConfirmarReserva.aspx?hab=${datos.habitacionId}&desayuno=${datos.conDesayuno}`;
});

cargarCarrito();

// ── Perfil ───────────────────────────────────────────────
const btnPerfil = document.getElementById("btn-perfil");
const panelPerfil = document.getElementById("panel-perfil");
const overlayPerfil = document.getElementById("overlay-perfil");
const btnCerrarPerfil = document.getElementById("btn-cerrar-perfil");

function abrirPerfil() {
    panelPerfil?.classList.add("active");
    overlayPerfil?.classList.add("active");
}

function cerrarPerfil() {
    panelPerfil?.classList.remove("active");
    overlayPerfil?.classList.remove("active");
}

btnPerfil?.addEventListener("click", abrirPerfil);
btnCerrarPerfil?.addEventListener("click", cerrarPerfil);
overlayPerfil?.addEventListener("click", cerrarPerfil);