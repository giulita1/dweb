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
            <div class="card-reserva <%# Eval("Estado").ToString() == "Cancelada" ? "cancelada" : "" %>">

                <div class="card-reserva-img">
                    <img src='<%# ResolveUrl("~/img/" + Eval("ImagenUrl")) %>'
                         alt='<%# Eval("NombreHabitacion") %>'>
                    <span class="badge-estado <%# Eval("Estado").ToString().ToLower() %>">
                        <%# Eval("Estado") %>
                    </span>
                </div>

                <div class="card-reserva-contenido">

                    <div class="reserva-tipo"><%# Eval("TipoHabitacion") %></div>
                    <h3><%# Eval("NombreHabitacion") %></h3>

                    <div class="reserva-fechas">
                        <div class="fecha-item">
                            <span class="material-symbols-outlined">login</span>
                            <div>
                                <small>Llegada</small>
                                <p><%# ((DateTime)Eval("FechaLlegada")).ToString("dd/MM/yyyy") %></p>
                            </div>
                        </div>
                        <div class="fecha-sep"></div>
                        <div class="fecha-item">
                            <span class="material-symbols-outlined">logout</span>
                            <div>
                                <small>Salida</small>
                                <p><%# ((DateTime)Eval("FechaSalida")).ToString("dd/MM/yyyy") %></p>
                            </div>
                        </div>
                    </div>

                    <div class="reserva-detalles">
                        <span>
                            <span class="material-symbols-outlined">nights_stay</span>
                            <%# ((DateTime)Eval("FechaSalida") - (DateTime)Eval("FechaLlegada")).Days %> noches
                        </span>
                        <span>
                            <span class="material-symbols-outlined">group</span>
                            <%# Eval("Huespedes") %> huéspedes
                        </span>
                        <% if ((bool)Eval("IncluyeDesayuno")) { %>
                        <span>
                            <span class="material-symbols-outlined">free_breakfast</span>
                            Desayuno
                        </span>
                        <% } %>
                    </div>

                   <div class="reserva-footer">
                        <div class="reserva-total">
                            <small>Total</small>
                            <span><%# string.Format("{0:C0}", Eval("Total")) %></span>
                        </div>
                        <asp:Button runat="server"
                            Text="Cancelar reserva"
                            CssClass="btn-cancelar"
                            CommandName="Cancelar"
                            CommandArgument='<%# Eval("Id_Reserva") %>'
                            Visible='<%# Eval("Estado").ToString() != "Cancelada" %>' />
                        <asp:Label runat="server"
                            Text="Reserva cancelada"
                            CssClass="lbl-cancelada"
                            Visible='<%# Eval("Estado").ToString() == "Cancelada" %>' />
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