<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="desarrolloweb.GestionUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/gestionUsuarios.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<section class="hero-gestion">
    <div class="hero-texto">
        <span class="subtitulo-section">ADMINISTRACIÓN</span>
        <h1>Gestión de Usuarios</h1>
        <p>Buscá, filtrá y administrá los usuarios del sistema.</p>
    </div>
</section>

<div class="contenedor-gestion">

    <%-- Filtros --%>
   <div class="filtros-gestion">

    <div class="filtro-busqueda">
        <span class="material-symbols-outlined">search</span>
        <input type="text" id="txtBusqueda" class="input-busqueda"
               placeholder="Buscar por nombre, apellido, email o usuario..."
               value="<%= Request.QueryString["busqueda"] ?? "" %>">
    </div>

    <div class="filtro-checks">
        <label class="check-item">
            <input type="checkbox" id="chkBloqueados"
                   <%= Request.QueryString["bloqueados"] == "1" ? "checked" : "" %>>
            <span>Solo bloqueados</span>
        </label>
    </div>

    <button type="button" class="btn-buscar-gestion" onclick="buscar()">
        Buscar
    </button>

</div>

    <%-- Contador --%>
    <asp:Label ID="lblContador" runat="server" CssClass="contador-resultados" />

    <%-- Tabla --%>
    <asp:Panel ID="pnlTabla" runat="server" Visible="false">
        <div class="tabla-wrapper">
            <table class="tabla-usuarios">
                <thead>
                    <tr>
                        <th>Usuario</th>
                        <th>Email</th>
                        <th>Rol</th>
                        <th>Intentos</th>
                        <th>Estado</th>
                        <th>Acción</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptUsuarios" runat="server"
                        OnItemCommand="rptUsuarios_ItemCommand">
                        <ItemTemplate>
                            <tr class='<%# (bool)Eval("Bloqueado") ? "fila-bloqueada" : "" %>'>
                                <td>
                                    <div class="usuario-info">
                                        <div class="usuario-avatar">
                                            <%# Eval("Nombre").ToString().Substring(0,1).ToUpper() %>
                                        </div>
                                        <div>
                                            <p class="usuario-nombre"><%# Eval("Nombre") %> <%# Eval("Apellido") %></p>
                                            <small class="usuario-user">@<%# Eval("User") %></small>
                                        </div>
                                    </div>
                                </td>
                                <td><%# Eval("Email") %></td>
                                <td>
                                    <span class='badge-rol <%# (int)Eval("IdRol") == 1 ? "admin" : "cliente" %>'>
                                        <%# (int)Eval("IdRol") == 1 ? "Admin" : "Cliente" %>
                                    </span>
                                </td>
                                <td class="td-intentos"><%# Eval("Intentos") %>/3</td>
                                <td>
                                    <span class='badge-estado <%# (bool)Eval("Bloqueado") ? "bloqueado" : "activo" %>'>
                                        <%# (bool)Eval("Bloqueado") ? "Bloqueado" : "Activo" %>
                                    </span>
                                </td>
                                <td>
                                    <asp:Button runat="server"
                                        Text="Desbloquear"
                                        CssClass="btn-desbloquear"
                                        CommandName="Desbloquear"
                                        CommandArgument='<%# Eval("Id_Usuario") %>'
                                        Visible='<%# (bool)Eval("Bloqueado") %>' />
                                    <span class="sin-accion"
                                        <%# !(bool)Eval("Bloqueado") ? "" : "style='display:none'" %>>
                                        —
                                    </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlSinResultados" runat="server" Visible="false" CssClass="sin-resultados">
        <span class="material-symbols-outlined">person_search</span>
        <h3>Sin resultados</h3>
        <p>No se encontraron usuarios con los filtros aplicados.</p>
    </asp:Panel>

</div>

</asp:Content>