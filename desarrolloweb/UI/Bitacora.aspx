<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="desarrolloweb.UI.Bitacora" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href='<%= ResolveUrl("~/Content/bitacora.css") %>' rel="stylesheet" type="text/css" />
    <style type="text/css">
        .js-filter-field {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bitacora-container">
        <h2 class="bitacora-title">Consulta de Bitácora del Sistema</h2>          
        <div class="filter-panel">
            <div class="filter-grid">                 
                <div class="form-group">
                    <label for="<%= ddlUsuario.ClientID %>">Usuario (Login)</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-control js-filter-field">
                        <asp:ListItem Text="-- Todos los usuarios --" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>                                                 
                <div class="form-group">
                    <label for="<%= ddlEvento.ClientID %>">Evento / Actividad</label>
                    <asp:DropDownList ID="ddlEvento" runat="server" CssClass="form-control js-filter-field">
                        <asp:ListItem Text="-- Todos los eventos --" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label for="<%= ddlCriticidad.ClientID %>">Criticidad</label>
                    <asp:DropDownList ID="ddlCriticidad" runat="server" CssClass="form-control js-filter-field">
                        <asp:ListItem Text="-- Todas --" Value=""></asp:ListItem>
                        <asp:ListItem Text="Baja" Value="Baja"></asp:ListItem>
                        <asp:ListItem Text="Media" Value="Media"></asp:ListItem>
                        <asp:ListItem Text="Alta" Value="Alta"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label for="<%= txtFechaDesde.ClientID %>">Fecha Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control js-filter-field" TextMode="Date" Height="19px"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label for="<%= txtFechaHasta.ClientID %>">Fecha Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control js-filter-field" TextMode="Date"></asp:TextBox>
                </div>
            </div>    
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>

            <div class="action-bar">
                <button type="button" class="btn btn-clear" onclick="limpiarFiltros();">Limpiar Campos</button>
                <button type="button" class="btn btn-print" onclick="window.print();">Imprimir Pantalla</button>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar Bitácora" CssClass="btn btn-filter" 
                    OnClick="btnFiltrar_Click" 
                    OnClientClick='<%# "return validarFechas(\"" + txtFechaDesde.ClientID + "\", \"" + txtFechaHasta.ClientID + "\");" %>' />
            </div>
        </div>
        
        <div class="table-container">
            <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="False" CssClass="grid-view" GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No se encontraron registros de bitácora con los filtros aplicados.">
                <Columns>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                    <asp:BoundField DataField="Nombre" HeaderText="Usuario (Nombre)" />
                    <asp:BoundField DataField="Actividad" HeaderText="Evento / Acción" />
                    <asp:BoundField DataField="Criticidad" HeaderText="Criticidad" />
                </Columns>
            </asp:GridView>
        </div>
        
    </div>
    <script src='<%= ResolveUrl("~/ScriptsJS/Bitacora.js") %>' type="text/javascript"></script>
</asp:Content>