<%@ Page Title="Integridad del Sistema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DigitoVerificador.aspx.cs" Inherits="desarrolloweb.DigitoVerificador" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/login.css") %>" />
    
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/integridad.css") %>" />
    
    <script defer src="<%= ResolveUrl("~/ScriptsJS/integridad.js") %>"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-integridad">
        <h2>Verificación de Integridad de Datos</h2>

        <asp:Panel ID="pnlMensajeOK" runat="server" Visible="false" CssClass="msg-ok">
            <span class="material-symbols-outlined" style="vertical-align: middle; margin-right: 8px;">verified</span>
            No se detectaron fallas de integridad en la base de datos. El sistema está seguro.
        </asp:Panel>

        <asp:GridView ID="dgvInfracciones" runat="server" AutoGenerateColumns="false" CssClass="grid-error" GridLines="None">
            <Columns>
                <asp:BoundField DataField="Tabla" HeaderText="Nombre de la Tabla" />
                <asp:BoundField DataField="IdRegistro" HeaderText="Fila Afectada (ID / COLUMNA)" />
                <asp:BoundField DataField="TipoError" HeaderText="Tipo de Error Detectado" />
            </Columns>
        </asp:GridView>

        <div class="acciones-container">
            <asp:Button ID="btnRecalcular" runat="server" Text="RECALCULAR DÍGITOS" CssClass="btn-login" style="background-color: var(--dorado); flex: 1; min-width: 200px;" OnClientClick="return confirmarRecalculo();" OnClick="btnRecalcular_Click" />
        </div>

        <div class="seccion-restore">
            <h2>Restauración del Sistema</h2>
            <div class="file-upload-wrapper">
                <label for="fuBackup">Seleccionar archivo de copia de seguridad (.bak)</label>
                <asp:FileUpload ID="fuBackup" runat="server" />
            </div>
            <asp:Button ID="btnRestore" runat="server" Text="REALIZAR RESTORE" CssClass="btn-login" style="background-color: #b91c1c; max-width: 250px;" OnClientClick="return confirmarRestore();" OnClick="btnRestore_Click" />
        </div>
    </div>
</asp:Content>