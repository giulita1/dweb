<%@ Page Title="Gestionar Idioma" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionIdioma.aspx.cs" Inherits="desarrolloweb.UI.GestionIdioma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/gestion-idioma.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<section class="hero-gestion">
    <div class="hero-texto">
        <span class="subtitulo-section">ADMINISTRACIÓN</span>
        <h1>Gestión de Idiomas</h1>
        <p>Agregá idiomas y traducí las etiquetas del sistema.</p>
    </div>
</section>

<div class="contenedor-gestion">

    <div class="card-gestion">
        <h2><asp:Literal ID="litTituloAgregarIdioma" runat="server" Text="Agregar Nuevo Idioma" /></h2>
        <div class="form-inline">
            <div class="form-grupo">
                <asp:Label ID="Label1" runat="server" Text="Idioma" />
                <asp:TextBox ID="txtIdioma" runat="server" CssClass="input-gestion" placeholder="Ej: Portugués" />
            </div>
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar"
                CssClass="btn-agregar" OnClick="btnAgregar_Click" />
        </div>
    </div>

    <div class="card-gestion">
        <h2><asp:Literal ID="litTituloGestionarIdioma" runat="server" Text="Gestionar Idioma" /></h2>

        <div class="form-inline">
            <div class="form-grupo">
                <asp:Label ID="Label2" runat="server" Text="Seleccionar Idioma" />
                <asp:DropDownList ID="ddlIdiomas" runat="server" CssClass="input-gestion"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlIdiomas_SelectedIndexChanged" />
            </div>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                CssClass="btn-secundario" OnClick="btnBuscar_Click" />
        </div>

        <div class="tabla-wrapper">
            <asp:GridView ID="gvTraducciones" runat="server"
                AutoGenerateColumns="False"
                AutoGenerateSelectButton="True"
                OnRowCommand="gvTraducciones_RowCommand"
                CssClass="tabla-gestion"
                GridLines="None">
                <SelectedRowStyle CssClass="fila-seleccionada" />
                <Columns>
                    <asp:BoundField DataField="Nombre_Etiqueta" HeaderText="Etiqueta" />
                    <asp:BoundField DataField="Traduccion" HeaderText="Traducción" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="panel-edicion">
            <div class="form-inline">
                <div class="form-grupo">
                    <asp:Label ID="Label3" runat="server" Text="Etiqueta" />
                    <asp:TextBox ID="txtEtiqueta" runat="server" CssClass="input-gestion" ReadOnly="true" />
                </div>
                <div class="form-grupo">
                    <asp:Label ID="Label4" runat="server" Text="Traducción" />
                    <asp:TextBox ID="txtTraduccion" runat="server" CssClass="input-gestion" />
                </div>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar cambios"
                    CssClass="btn-agregar" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </div>

</div>

</asp:Content>