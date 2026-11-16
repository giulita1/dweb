<%@ Page Title="Gestionar Idioma" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionIdioma.aspx.cs" Inherits="desarrolloweb.UI.GestionIdioma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/gestion-idioma.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h2><asp:Literal ID="litTituloAgregarIdioma" runat="server" Text="Agregar Nuevo Idioma" /></h2>
        <asp:Label ID="Label1" runat="server" Text="Idioma: " />
        <asp:TextBox ID="txtIdioma" runat="server" />
        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
    </div>

    <hr />

    <div>
        <h2><asp:Literal ID="litTituloGestionarIdioma" runat="server" Text="Gestionar Idioma" /></h2>
        <asp:Label ID="Label2" runat="server" Text="Seleccionar Idioma: " />
        <asp:DropDownList ID="ddlIdiomas" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlIdiomas_SelectedIndexChanged" />
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-secondary" OnClick="btnBuscar_Click" />

        <br /><br />
        <asp:GridView ID="gvTraducciones" runat="server" 
            AutoGenerateColumns="False" 
            AutoGenerateSelectButton="True"
            OnRowCommand="gvTraducciones_RowCommand"
            CssClass="table">
            <Columns>
                <asp:BoundField DataField="Nombre_Etiqueta" HeaderText="Etiqueta" />
                <asp:BoundField DataField="Traduccion" HeaderText="Traducción" />
            </Columns>
        </asp:GridView>

        <br />
        <asp:Label ID="Label3" runat="server" Text="Etiqueta: " />
        <asp:TextBox ID="txtEtiqueta" runat="server" ReadOnly="true" />
        <asp:Label ID="Label4" runat="server" Text="Traducción: " />
        <asp:TextBox ID="txtTraduccion" runat="server" />
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
    </div>
</asp:Content>