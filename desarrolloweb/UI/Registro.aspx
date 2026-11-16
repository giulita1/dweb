<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="desarrolloweb.UI.Registro" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/registro.css") %>" />
    <script src="<%= ResolveUrl("~/ScriptsJS/registro.js") %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">    
    <div class="form-registro">
    <h1><asp:Literal ID="litTituloCrearCuenta" runat="server" Text="Crear Cuenta" /></h1>
      <div class="datos-container">
      <div class="datos-izquierda">
      <div class="input-container">
      <label for="input-nombre"><asp:Literal ID="litLblNombreRegistro" runat="server" Text="Nombre" /></label>
          <input type="text" id="input-nombre" name="nombre" value="<%= Request.Form["nombre"] ?? "" %>">
          <asp:Label ID="lblNombre" CssClass="error-msj" runat="server" ></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-apellido"><asp:Literal ID="litLblApellidoRegistro" runat="server" Text="Apellido" /></label>
          <input type="text" id="input-apellido" name="apellido" value="<%= Request.Form["apellido"] ?? "" %>">
          <asp:Label ID="lblApellido" CssClass="error-msj" runat="server"></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-email"><asp:Literal ID="litLblEmailRegistro" runat="server" Text="Email" /></label>
          <input type="email" id="input-email" name="email" value="<%= Request.Form["email"] ?? "" %>">
          <asp:Label ID="lblEmail" CssClass="error-msj" runat="server" ></asp:Label>
      </div>
      </div>

      <div class="datos-derecha">
      <div class="input-container">
      <label for="input-usuario"><asp:Literal ID="litLblUsuarioRegistro" runat="server" Text="Nombre de usuario" /></label>
          <input type="text" id="input-usuario" name="usuario" value="<%= Request.Form["usuario"] ?? "" %>">
          <asp:Label ID="lblUsuario" CssClass="error-msj" runat="server"></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-contrasena"><asp:Literal ID="litLblContrasenaRegistro" runat="server" Text="Contraseña" /></label>
          <input type="password" id="input-contrasena" name="contrasena">
          <asp:Label ID="lblContra" CssClass="error-msj" runat="server" ></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-confirmar-contra"><asp:Literal ID="litLblConfirmarContra" runat="server" Text="Confirmar contraseña" /></label>
          <input type="password" id="input-confirmar-contra" name="confirmarContrasena">
         <asp:Label ID="lblConfirmar" CssClass="error-msj" runat="server" ></asp:Label>
      </div>
      </div>
      </div>

        <div class="container-texto">
      <span><asp:HyperLink ID="lnkLoginRegistro" runat="server" NavigateUrl="~/UI/Login.aspx" Text="¿Ya tenés cuenta? Iniciá sesión" /></span>

      <asp:Label ID="lblErrorGeneral" runat="server" CssClass="error-msj" />

        </div>
      <input type="submit" class="btn-registrarse" value="REGISTRARSE">
        </div>
</asp:Content>