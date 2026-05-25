document.addEventListener("DOMContentLoaded", () => {

    const campos = [
        { inputId: "input-nombre", lblId: "lblNombre" },
        { inputId: "input-apellido", lblId: "lblApellido" },
        { inputId: "input-email", lblId: "lblEmail" },
        { inputId: "input-usuario", lblId: "lblUsuario" },
        { inputId: "input-contrasena", lblId: "lblContra" },
        { inputId: "input-confirmar-contra", lblId: "lblConfirmar" }
    ];

    campos.forEach((campo) => {
        var input = document.getElementById(campo.inputId);
        var lbl = document.getElementById(campo.lblId);

        if (lbl && input && lbl.innerText.trim() !== "") {
            input.classList.add("input-error");
        }

        input.addEventListener("input", function () {
            input.classList.remove("input-error");
        });
    });

});