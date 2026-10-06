<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
AutoEventWireup="true" CodeBehind="GestionPerfiles.aspx.cs"
Inherits="desarrolloweb.UI.GestionPerfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/gestionPerfiles.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<section class="hero-gestion">
    <div class="hero-texto">
        <span class="subtitulo-section">ADMINISTRACIÓN</span>
        <h1>Gestión de Perfiles</h1>
        <p>Administrá perfiles, familias y patentes del sistema.</p>
    </div>
</section>

<div class="contenedor-gestion">

    <asp:Label ID="lblMensaje" runat="server" CssClass="lbl-mensaje" Visible="false" />

    <div class="tabs">
        <button type="button" class="tab active" onclick="cambiarTab('perfiles', this)">Perfiles</button>
        <button type="button" class="tab" onclick="cambiarTab('familias', this)">Familias</button>
        <button type="button" class="tab" onclick="cambiarTab('patentes', this)">Patentes</button>
    </div>

 
    <div id="tab-perfiles" class="tab-content active">

        <div class="card-gestion">
            <h2>Nuevo perfil</h2>
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Nombre</label>
                    <asp:TextBox ID="txtNombrePerfil" runat="server" CssClass="input-gestion" placeholder="Ej: Administrador" />
                </div>
                <div class="form-grupo">
                    <label>Descripción</label>
                    <asp:TextBox ID="txtDescPerfil" runat="server" CssClass="input-gestion" placeholder="Descripción opcional" />
                </div>
                <asp:Button ID="btnAgregarPerfil" runat="server" Text="Agregar"
                    CssClass="btn-agregar" OnClick="btnAgregarPerfil_Click" />
            </div>
        </div>

        <div class="card-gestion">
            <h2>Perfiles existentes</h2>
            <asp:Repeater ID="rptPerfiles" runat="server" OnItemCommand="rptPerfiles_ItemCommand">
                <HeaderTemplate><div class="lista-items"></HeaderTemplate>
                <ItemTemplate>
                    <div class="item-row">
                        <div class="item-info">
                            <span class="item-nombre"><%# Eval("Nombre") %></span>
                            <span class="item-desc"><%# Eval("Descripcion") %></span>
                        </div>
                        <div class="item-acciones">
                            <asp:Button runat="server" Text="Ver árbol"
                                CssClass="btn-ver-arbol"
                                CommandName="VerArbol"
                                CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button runat="server" Text="Agregar familia"
                                CssClass="btn-secundario"
                                CommandName="AbrirAgregarFamilia"
                                CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button runat="server" Text="Agregar patente"
                                CssClass="btn-secundario"
                                CommandName="AbrirAgregarPatente"
                                CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button runat="server" Text="Eliminar"
                                CssClass="btn-eliminar"
                                CommandName="EliminarPerfil"
                                CommandArgument='<%# Eval("Id") %>'
                                OnClientClick="return confirm('¿Eliminar este perfil?')" />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
        </div>

        <asp:Panel ID="pnlAgregarFamiliaAPerfil" runat="server" Visible="false" CssClass="card-gestion panel-accion">
            <h2>Agregar familia al perfil <asp:Label ID="lblNombrePerfilSeleccionado" runat="server" CssClass="highlight" /></h2>
            <asp:HiddenField ID="hfIdPerfilSeleccionado" runat="server" />
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Familia</label>
                    <asp:DropDownList ID="ddlFamiliasParaPerfil" runat="server" CssClass="input-gestion" />
                </div>
                <asp:Button ID="btnConfirmarFamiliaAPerfil" runat="server" Text="Confirmar"
                    CssClass="btn-agregar" OnClick="btnConfirmarFamiliaAPerfil_Click" />
                <asp:Button ID="btnCancelarFamiliaAPerfil" runat="server" Text="Cancelar"
                    CssClass="btn-cancelar" OnClick="btnCancelar_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlAgregarPatenteAPerfil" runat="server" Visible="false" CssClass="card-gestion panel-accion">
            <h2>Agregar patente al perfil <asp:Label ID="lblNombrePerfilSeleccionado2" runat="server" CssClass="highlight" /></h2>
            <asp:HiddenField ID="hfIdPerfilSeleccionado2" runat="server" />
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Patente</label>
                    <asp:DropDownList ID="ddlPatentesParaPerfil" runat="server" CssClass="input-gestion" />
                </div>
                <asp:Button ID="btnConfirmarPatenteAPerfil" runat="server" Text="Confirmar"
                    CssClass="btn-agregar" OnClick="btnConfirmarPatenteAPerfil_Click" />
                <asp:Button ID="btnCancelarPatenteAPerfil" runat="server" Text="Cancelar"
                    CssClass="btn-cancelar" OnClick="btnCancelar_Click" />
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlArbol" runat="server" Visible="false" CssClass="card-gestion">
            <h2>Árbol del perfil: <asp:Label ID="lblNombreArbol" runat="server" CssClass="highlight" /></h2>
            <div class="arbol-container">
                <asp:Literal ID="litArbol" runat="server" />
            </div>
        </asp:Panel>

    </div>

    <div id="tab-familias" class="tab-content">

        <div class="card-gestion">
            <h2>Nueva familia</h2>
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Nombre</label>
                    <asp:TextBox ID="txtNombreFamilia" runat="server" CssClass="input-gestion" placeholder="Ej: Seguridad" />
                </div>
                <div class="form-grupo">
                    <label>Descripción</label>
                    <asp:TextBox ID="txtDescFamilia" runat="server" CssClass="input-gestion" placeholder="Descripción opcional" />
                </div>
                <asp:Button ID="btnAgregarFamilia" runat="server" Text="Agregar"
                    CssClass="btn-agregar" OnClick="btnAgregarFamilia_Click" />
            </div>
        </div>

        <div class="card-gestion">
            <h2>Familias existentes</h2>
            <asp:Repeater ID="rptFamilias" runat="server" OnItemCommand="rptFamilias_ItemCommand">
                <HeaderTemplate><div class="lista-items"></HeaderTemplate>
                <ItemTemplate>
                    <div class="item-row">
                        <div class="item-info">
                            <span class="item-nombre"><%# Eval("Nombre") %></span>
                            <span class="item-desc"><%# Eval("Descripcion") %></span>
                        </div>
                        <div class="item-acciones">
                            <asp:Button runat="server" Text="Agregar subfamilia"
                                CssClass="btn-secundario"
                                CommandName="AbrirAgregarSubfamilia"
                                CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button runat="server" Text="Agregar patente"
                                CssClass="btn-secundario"
                                CommandName="AbrirAgregarPatenteAFamilia"
                                CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button runat="server" Text="Eliminar"
                                CssClass="btn-eliminar"
                                CommandName="EliminarFamilia"
                                CommandArgument='<%# Eval("Id") %>'
                                OnClientClick="return confirm('¿Eliminar esta familia?')" />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
        </div>

        <asp:Panel ID="pnlAgregarSubfamilia" runat="server" Visible="false" CssClass="card-gestion panel-accion">
            <h2>Agregar subfamilia a <asp:Label ID="lblNombreFamiliaSeleccionada" runat="server" CssClass="highlight" /></h2>
            <asp:HiddenField ID="hfIdFamiliaSeleccionada" runat="server" />
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Subfamilia</label>
                    <asp:DropDownList ID="ddlSubfamilias" runat="server" CssClass="input-gestion" />
                </div>
                <asp:Button ID="btnConfirmarSubfamilia" runat="server" Text="Confirmar"
                    CssClass="btn-agregar" OnClick="btnConfirmarSubfamilia_Click" />
                <asp:Button ID="btnCancelarSubfamilia" runat="server" Text="Cancelar"
                    CssClass="btn-cancelar" OnClick="btnCancelar_Click" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAgregarPatenteAFamilia" runat="server" Visible="false" CssClass="card-gestion panel-accion">
            <h2>Agregar patente a <asp:Label ID="lblNombreFamiliaSeleccionada2" runat="server" CssClass="highlight" /></h2>
            <asp:HiddenField ID="hfIdFamiliaSeleccionada2" runat="server" />
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Patente</label>
                    <asp:DropDownList ID="ddlPatentesParaFamilia" runat="server" CssClass="input-gestion" />
                </div>
                <asp:Button ID="btnConfirmarPatenteAFamilia" runat="server" Text="Confirmar"
                    CssClass="btn-agregar" OnClick="btnConfirmarPatenteAFamilia_Click" />
                <asp:Button ID="btnCancelarPatenteAFamilia" runat="server" Text="Cancelar"
                    CssClass="btn-cancelar" OnClick="btnCancelar_Click" />
            </div>
        </asp:Panel>

    </div>

    <div id="tab-patentes" class="tab-content">

        <div class="card-gestion">
            <h2>Nueva patente</h2>
            <div class="form-inline">
                <div class="form-grupo">
                    <label>Nombre</label>
                    <asp:TextBox ID="txtNombrePatente" runat="server" CssClass="input-gestion" placeholder="Ej: GestionUsuarios" />
                </div>
                <div class="form-grupo">
                    <label>Descripción</label>
                    <asp:TextBox ID="txtDescPatente" runat="server" CssClass="input-gestion" placeholder="Descripción opcional" />
                </div>
                <asp:Button ID="btnAgregarPatente" runat="server" Text="Agregar"
                    CssClass="btn-agregar" OnClick="btnAgregarPatente_Click" />
            </div>
        </div>

        <div class="card-gestion">
            <h2>Patentes existentes</h2>
            <asp:Repeater ID="rptPatentes" runat="server" OnItemCommand="rptPatentes_ItemCommand">
                <HeaderTemplate><div class="lista-items"></HeaderTemplate>
                <ItemTemplate>
                    <div class="item-row">
                        <div class="item-info">
                            <span class="item-nombre"><%# Eval("Nombre") %></span>
                            <span class="item-desc"><%# Eval("Descripcion") %></span>
                        </div>
                        <div class="item-acciones">
                            <asp:Button runat="server" Text="Eliminar"
                                CssClass="btn-eliminar"
                                CommandName="EliminarPatente"
                                CommandArgument='<%# Eval("Id") %>'
                                OnClientClick="return confirm('¿Eliminar esta patente?')" />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
        </div>

    </div>

</div>

<script>
function cambiarTab(nombre, btn) {
    document.querySelectorAll('.tab-content').forEach(t => t.classList.remove('active'));
    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
    document.getElementById('tab-' + nombre).classList.add('active');
    btn.classList.add('active');
}
</script>

</asp:Content>