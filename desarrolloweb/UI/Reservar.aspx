<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
AutoEventWireup="true" CodeBehind="Reservar.aspx.cs"
Inherits="desarrolloweb.UI.Reservar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/reservar.css") %>" />
    <script defer src="<%= ResolveUrl("~/ScriptsJS/reservar.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<section class="hero-reserva">
    <div class="hero-reserva-texto">
        <span class="subtitulo-section">RESERVAS</span>
        <h1>Encontrá tu habitación ideal</h1>
        <p>Seleccioná tus fechas y te mostramos las habitaciones disponibles para tu estadía.</p>
    </div>
</section>

<div class="contenedor-reserva">

    <div class="filtros-reserva">
       
        <div class="filtros">

    <div class="input-container">
        <label>Fecha de llegada</label>
        <input type="date" id="fechaLlegada" class="input-date"
               value="<%= Request.QueryString["llegada"] %>">
    </div>

    <div class="input-container">
        <label>Fecha de salida</label>
        <input type="date" id="fechaSalida" class="input-date"
               value="<%= Request.QueryString["salida"] %>">
    </div>

    <div class="input-container">
        <label>Huéspedes</label>
        <div class="input-huespedes-wrapper">
            <button type="button" class="btn-cantidad" id="btn-menos-r">−</button>
            <span id="display-huespedes-r">
                <%= Request.QueryString["huespedes"] ?? "1" %>
            </span>
            <input type="hidden" id="cant-huespedes-r" 
                   value="<%= Request.QueryString["huespedes"] ?? "1" %>">
            <button type="button" class="btn-cantidad" id="btn-mas-r">+</button>
        </div>
    </div>

    <button type="button" class="btn-buscar" onclick="buscar()">
        Buscar disponibilidad
    </button>

</div>

        <asp:Label ID="lblMensaje" runat="server"
            CssClass="lbl-mensaje" Visible="false" />
    </div>

    <asp:Panel ID="pnlResultados" runat="server" Visible="false">

        <div class="titulo-seccion">
            <h2>Habitaciones disponibles</h2>
            <asp:Label ID="lblSubtitulo" runat="server" CssClass="subtitulo-resultado" />
        </div>

        <asp:Repeater ID="rptHabitaciones" runat="server">
            <HeaderTemplate>
                <div class="habitaciones-grid">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="card-habitacion">

                    <div class="card-img-wrapper">
                        <img src='<%# ResolveUrl("~/img/" + Eval("ImagenUrl")) %>'
                             alt='<%# Eval("Nombre") %>'
                             onerror="this.src='<%# ResolveUrl("~/img/habitacion1.jpg") %>'">
                        <span class="badge-tipo"><%# Eval("Tipo") %></span>
                    </div>

                    <div class="contenido-card">
                        <h3><%# Eval("Nombre") %></h3>
                        <p><%# Eval("Descripcion") %></p>

                       
                        <div class="footer-card">
                            <div class="precio-wrapper">
                                <span class="precio"><%# string.Format("{0:C0}", Eval("PrecioPorNoche")) %></span>
                                <span class="precio-label">/ noche</span>
                            </div>
                            <button type="button" class="btn-seleccionar"
                                onclick="seleccionarHabitacion(
                                    '<%# Eval("HabitacionId") %>',
                                    '<%# Eval("Nombre") %>',
                                    '<%# Eval("Tipo") %>',
                                    <%# Eval("PrecioPorNoche") %>,
                                    '<%# ResolveUrl("~/img/" + Eval("ImagenUrl")) %>'
                                )">
                                Seleccionar
                            </button>
                        </div>
                    </div>

                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlSinResultados" runat="server" Visible="false" CssClass="sin-resultados">
            <span class="material-symbols-outlined">hotel</span>
            <h3>Sin disponibilidad</h3>
            <p>No hay habitaciones disponibles para las fechas y cantidad de huéspedes seleccionados. Probá con otras fechas.</p>
        </asp:Panel>

    </asp:Panel>

</div>

</asp:Content>