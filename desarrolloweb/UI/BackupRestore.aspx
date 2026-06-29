<%@ Page Title="Backup y Restore" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BackupRestore.aspx.cs" Inherits="desarrolloweb.UI.BackupRestore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/backup-restore.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="br-container">
        <h2 class="br-header">Gestión de Base de Datos</h2>
        
        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </asp:Panel>

        <div class="cards-wrapper">
            <div class="br-card">
                <h3>Realizar Backup</h3>
                <p>Genera una copia de seguridad completa de la base de datos actual. El archivo se guardará automáticamente en el servidor del proyecto.</p>
                <asp:Button ID="btnBackup" runat="server" Text="Generar Backup" CssClass="btn-custom btn-blue" OnClick="btnBackup_Click" />
            </div>

            <div class="br-card">
                <h3>Realizar Restore</h3>
                <p>Seleccione un archivo de copia de seguridad del servidor para restaurar el sistema. <strong>Atención:</strong> Esta acción sobreescribirá los datos actuales.</p>
                
                <div class="file-upload-box">
                    <asp:DropDownList ID="ddlBackups" runat="server" CssClass="ddl-custom"></asp:DropDownList>
                </div>
                
                <asp:Button ID="btnRestore" runat="server" Text="Restaurar Base de Datos" CssClass="btn-custom btn-green" OnClick="btnRestore_Click" 
                    OnClientClick="return confirm('¿Está completamente seguro de que desea sobreescribir la base de datos actual? Esta acción no se puede deshacer.');" />
            </div>
        </div>
    </div>
</asp:Content>