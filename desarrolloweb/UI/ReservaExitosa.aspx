<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
AutoEventWireup="true" CodeBehind="ReservaExitosa.aspx.cs"
Inherits="desarrolloweb.UI.ReservaExitosa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/ReservaExitosa.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        sessionStorage.removeItem("reserva");
</script>
<div class="contenedor-exitosa">

    <div class="card-exitosa">

        <div class="icono-exito">
            <span class="material-symbols-outlined">check_circle</span>
        </div>

        <span class="subtitulo-section">RESERVA CONFIRMADA</span>
        <h1>¡Tu estadía está reservada!</h1>
        <p>Recibimos tu reserva correctamente. Te esperamos en Hotel Patagonia.</p>

        <div class="detalle-exitosa">
            <div class="detalle-item">
                <span class="material-symbols-outlined">bed</span>
                <span id="lblNombreHab" runat="server"></span>
            </div>
            <div class="detalle-item">
                <span class="material-symbols-outlined">login</span>
                <span id="lblLlegada" runat="server"></span>
            </div>
            <div class="detalle-item">
                <span class="material-symbols-outlined">logout</span>
                <span id="lblSalida" runat="server"></span>
            </div>
            <div class="detalle-item">
                <span class="material-symbols-outlined">payments</span>
                <span id="lblTotal" runat="server"></span>
            </div>
        </div>

        <div class="exitosa-acciones">
            <a href="<%= ResolveUrl("~/UI/MisReservas.aspx") %>" class="btn-mis-reservas">
                Ver mis reservas
            </a>
            <a href="<%= ResolveUrl("~/UI/Inicio.aspx") %>" class="btn-inicio">
                Volver al inicio
            </a>
        </div>

    </div>

</div>

</asp:Content>