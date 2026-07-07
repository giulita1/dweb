<%@ Page Title="Confirmar Reserva - Hotel Patagonia" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmarReserva.aspx.cs" Inherits="desarrolloweb.UI.ConfirmarReserva" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/confirmarReserva.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="contenedor-confirmar">

    <%-- Columna izquierda: resumen --%>
    <div class="resumen-reserva">

        <span class="subtitulo-section">RESUMEN DE RESERVA</span>
        <h1>Tu estadía</h1>

        <div class="resumen-card">

            <div class="resumen-img-wrapper">
                <img id="imgHabitacion" runat="server" src="" alt="Habitación">
                <span class="resumen-badge" id="lblTipo" runat="server"></span>
            </div>

            <div class="resumen-info">

                <h2 id="lblNombre" runat="server"></h2>

                <div class="resumen-fechas">
                    <div class="fecha-item">
                        <span class="material-symbols-outlined">login</span>
                        <div>
                            <small>Llegada</small>
                            <p id="lblLlegada" runat="server"></p>
                        </div>
                    </div>
                    <div class="fecha-separador"></div>
                    <div class="fecha-item">
                        <span class="material-symbols-outlined">logout</span>
                        <div>
                            <small>Salida</small>
                            <p id="lblSalida" runat="server"></p>
                        </div>
                    </div>
                </div>

                <div class="resumen-detalles">
                    <div class="detalle-item">
                        <span class="material-symbols-outlined">nights_stay</span>
                        <span id="lblNoches" runat="server"></span>
                    </div>
                    <div class="detalle-item">
                        <span class="material-symbols-outlined">group</span>
                        <span id="lblHuespedes" runat="server"></span>
                    </div>
                </div>

            </div>

        </div>

        <%-- Desglose de precios --%>
        <div class="desglose">

            <div class="desglose-fila">
                <span id="lblDescHab" runat="server"></span>
                <span id="lblPrecioHab" runat="server"></span>
            </div>

            <asp:Panel ID="pnlDesayuno" runat="server" Visible="false"
                CssClass="desglose-fila">
                <span id="lblDescDesayuno" runat="server"></span>
                <span id="lblPrecioDesayuno" runat="server"></span>
            </asp:Panel>

            <div class="desglose-total">
                <span>Total</span>
                <span id="lblTotal" runat="server"></span>
            </div>

        </div>

    </div>

    <%-- Columna derecha: pago --%>
    <div class="formulario-pago">

        <span class="subtitulo-section">DATOS DE PAGO</span>
        <h1>Completá tu reserva</h1>

        <div class="form-grupo">
            <label>Nombre en la tarjeta</label>
            <input type="text" id="txtNombreTarjeta" class="input-pago"
                   placeholder="Ej: JUAN PEREZ" maxlength="50">
        </div>

        <div class="form-grupo">
            <label>Número de tarjeta</label>
            <div class="input-tarjeta-wrapper">
                <input type="text" id="txtNumeroTarjeta" class="input-pago"
                       placeholder="0000 0000 0000 0000" maxlength="19">
                <span class="material-symbols-outlined icono-tarjeta">credit_card</span>
            </div>
        </div>

        <div class="form-fila">
            <div class="form-grupo">
                <label>Vencimiento</label>
                <input type="text" id="txtVencimiento" class="input-pago"
                       placeholder="MM/AA" maxlength="5">
            </div>
            <div class="form-grupo">
                <label>CVV</label>
                <input type="text" id="txtCvv" class="input-pago"
                       placeholder="***" maxlength="3">
            </div>
        </div>

        <div class="form-grupo">
            <label>Email de confirmación</label>
            <input type="email" id="txtEmail" class="input-pago"
                   placeholder="tu@email.com">
        </div>

        <div class="seguro-pago">
            <span class="material-symbols-outlined">lock</span>
            <p>Pago simulado — tus datos no serán procesados ni almacenados.</p>
        </div>

        <asp:Button ID="btnPagar" runat="server"
            Text="CONFIRMAR Y PAGAR"
            CssClass="btn-pagar"
            OnClick="btnPagar_Click" />

        <asp:Label ID="lblError" runat="server"
            CssClass="lbl-error" Visible="false" />

    </div>

</div>

<script>
    // Formateo automático número de tarjeta
    document.getElementById('txtNumeroTarjeta').addEventListener('input', function () {
        let val = this.value.replace(/\D/g, '').substring(0, 16);
        this.value = val.replace(/(.{4})/g, '$1 ').trim();
    });

    // Formateo automático vencimiento
    document.getElementById('txtVencimiento').addEventListener('input', function () {
        let val = this.value.replace(/\D/g, '').substring(0, 4);
        if (val.length >= 2) val = val.substring(0, 2) + '/' + val.substring(2);
        this.value = val;
    });

    // Solo números en CVV
    document.getElementById('txtCvv').addEventListener('input', function () {
        this.value = this.value.replace(/\D/g, '').substring(0, 3);
    });
</script>

</asp:Content>