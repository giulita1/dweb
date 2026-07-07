<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
AutoEventWireup="true" CodeBehind="MisReservas.aspx.cs"
Inherits="desarrolloweb.UI.MisReservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/misReservas.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<section class="hero-misreservas">
    <div class="hero-texto">
        <span class="subtitulo-section">MI CUENTA</span>
        <h1>Mis Reservas</h1>
        <p>Revisá y gestioná tus reservas activas.</p>
    </div>
</section>

<div class="contenedor-misreservas">

    <asp:Panel ID="pnlSinReservas" runat="server" Visible="false" CssClass="sin-reservas">
        <span class="material-symbols-outlined">hotel</span>
        <h3>No tenés reservas todavía</h3>
        <p>Cuando hagas una reserva aparecerá acá.</p>
        <a href="<%= ResolveUrl("~/UI/Reservar.aspx") %>" class="btn-ir-reservar">
            Buscar habitaciones
        </a>
    </asp:Panel>

    <asp:Repeater ID="rptReservas" runat="server"
        OnItemCommand="rptReservas_ItemCommand">
        <HeaderTemplate>
            <div class="reservas-grid">
        </HeaderTemplate> 
       <ItemTemplate>
    <div class="card-reserva <%# ((BE.Reserva)Container.DataItem).Estado == "Cancelada" ? "cancelada" : "" %>">

        <div class="card-reserva-contenido">

            <div class="reserva-tipo"><%# ((BE.Reserva)Container.DataItem).Hab.Tipo %></div>
            <h3><%# ((BE.Reserva)Container.DataItem).Hab.Nombre %></h3>

            <div class="reserva-fechas">
                <div class="fecha-item">
                    <span class="material-symbols-outlined">login</span>
                    <div>
                        <small>Llegada</small>
                        <p><%# ((BE.Reserva)Container.DataItem).FechaLlegada.ToString("dd/MM/yyyy") %></p>
                    </div>
                </div>
                <div class="fecha-sep"></div>
                <div class="fecha-item">
                    <span class="material-symbols-outlined">logout</span>
                    <div>
                        <small>Salida</small>
                        <p><%# ((BE.Reserva)Container.DataItem).FechaSalida.ToString("dd/MM/yyyy") %></p>
                    </div>
                </div>
            </div>

            <div class="reserva-detalles">
                <span>
                    <span class="material-symbols-outlined">nights_stay</span>
                    <%# (((BE.Reserva)Container.DataItem).FechaSalida - ((BE.Reserva)Container.DataItem).FechaLlegada).Days %> noches
                </span>
                <span>
                    <span class="material-symbols-outlined">group</span>
                    <%# ((BE.Reserva)Container.DataItem).Huespedes %> huéspedes
                </span>
                <%# ((BE.Reserva)Container.DataItem).IncluyeDesayuno ? 
                    "<span><span class='material-symbols-outlined'>free_breakfast</span> Desayuno</span>" : "" %>
            </div>

            <div class="reserva-footer">
                <div class="reserva-total">
                    <small>Total</small>
                    <span><%# ((BE.Reserva)Container.DataItem).Total.ToString("C0", new System.Globalization.CultureInfo("es-AR")) %></span>
                </div>
                <asp:Button runat="server"
                    Text="Cancelar reserva"
                    CssClass="btn-cancelar"
                    CommandName="Cancelar"
                    CommandArgument='<%# ((BE.Reserva)Container.DataItem).Id_Reserva %>'
                    Visible='<%# ((BE.Reserva)Container.DataItem).Estado != "Cancelada" %>' />
                <asp:Label runat="server"
                    Text="Reserva cancelada"
                    CssClass="lbl-cancelada"
                    Visible='<%# ((BE.Reserva)Container.DataItem).Estado == "Cancelada" %>' />
            </div>

        </div>

    </div>
</ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>

</div>

</asp:Content>