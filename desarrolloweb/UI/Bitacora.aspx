<%@ Page Title="Consulta de Bitácora" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="desarrolloweb.UI.Bitacora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href='<%= ResolveUrl("~/Content/bitacora.css") %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bitacora-container">
        <h2 class="bitacora-title">Consulta de Bitácora del Sistema</h2>          
        
        <div class="filter-panel">
            <div class="filter-grid">                 
                <div class="form-group">
                    <label for="<%= ddlUsuario.ClientID %>">Usuario (Login)</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-control js-filter-field">
                    </asp:DropDownList>
                </div>                                                 
                
                <div class="form-group">
                    <label for="<%= ddlModulo.ClientID %>">Módulo</label>
                    <asp:DropDownList ID="ddlModulo" runat="server" CssClass="form-control js-filter-field">
                        <asp:ListItem Text="-- Todos los módulos --" Value="Todos"></asp:ListItem>
                        <asp:ListItem Text="Usuarios" Value="Usuarios"></asp:ListItem>
                        <asp:ListItem Text="Reservas" Value="Reservas"></asp:ListItem>
                        <asp:ListItem Text="Roles" Value="Roles"></asp:ListItem>
                        <asp:ListItem Text="Administracion" Value="Administracion"></asp:ListItem>
                        <asp:ListItem Text="Seguridad" Value="Seguridad"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="<%= ddlEvento.ClientID %>">Evento / Actividad</label>
                    <asp:DropDownList ID="ddlEvento" runat="server" CssClass="form-control js-filter-field">
                        <asp:ListItem Text="-- Todos los eventos --" Value="Todos"></asp:ListItem>
                        <asp:ListItem Text="Inicio de sesión exitoso" Value="Inicio de sesión exitoso"></asp:ListItem>
                        <asp:ListItem Text="Cierre de sesión exitoso" Value="Cierre de sesión exitoso"></asp:ListItem>
                        <asp:ListItem Text="Bloqueo preventivo de cuenta" Value="Bloqueo preventivo de cuenta"></asp:ListItem>
                        <asp:ListItem Text="Usuario registrado" Value="Usuario registrado"></asp:ListItem>
                        <asp:ListItem Text="Usuario desbloqueado" Value="Usuario desbloqueado"></asp:ListItem>
                        <asp:ListItem Text="Recálculo masivo de DVH y DVV" Value="Recálculo masivo de DVH y DVV"></asp:ListItem>
                        <asp:ListItem Text="Recuperación de contraseña solicitada" Value="Recuperación de contraseña solicitada"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label for="<%= ddlCriticidad.ClientID %>">Criticidad</label>
                    <asp:DropDownList ID="ddlCriticidad" runat="server" CssClass="form-control js-filter-field">
                        <asp:ListItem Text="-- Todas --" Value="Todos"></asp:ListItem>
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
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
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar Bitácora" CssClass="btn btn-filter" OnClick="btnFiltrar_Click" />
            </div>
        </div>
        
        <div class="table-container">
            <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="False" CssClass="grid-view" 
                GridLines="None" ShowHeaderWhenEmpty="true" 
                EmptyDataText="No se encontraron registros de bitácora con los filtros aplicados."
                AllowPaging="True" PageSize="10" OnPageIndexChanging="gvBitacora_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                    <asp:BoundField DataField="Nombre" HeaderText="Usuario (Login)" />
                    <asp:BoundField DataField="Modulo" HeaderText="Módulo" />
                    <asp:BoundField DataField="Actividad" HeaderText="Evento / Acción" />
                    <asp:BoundField DataField="Criticidad" HeaderText="Criticidad" />
                </Columns>
                <PagerStyle CssClass="paginacion-grid" />
            </asp:GridView>
        </div>
        
    </div>
    <script src='<%= ResolveUrl("~/ScriptsJS/Bitacora.js") %>' type="text/javascript"></script>
</asp:Content>