<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="desarrolloweb.UI.Registro" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/registro.css") %>" />
    <script src="<%= ResolveUrl("~/ScriptsJS/registro.js") %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">    
    <div class="form-registro">
      <div class="datos-container">
      <div class="datos-izquierda">
      <div class="input-container">
      <label for="input-nombre">Nombre</label>
          <input type="text" id="input-nombre" name="nombre">
          <asp:Label ID="lblNombre" CssClass="error-msj" runat="server" Visible="false"></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-apellido">Apellido</label>
          <input type="text" id="input-apellido" name="apellido">
          <asp:Label ID="lblApellido" CssClass="error-msj" runat="server" Visible="false"></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-email">Email</label>
          <input type="email" id="input-email" name="email">
          <asp:Label ID="lblEmail" CssClass="error-msj" runat="server" Visible="false"></asp:Label>
      </div>
      </div>

      <div class="datos-derecha">
      <div class="input-container">
      <label for="input-usuario">Nombre de usuario</label>
          <input type="text" id="input-usuario" name="usuario">
          <asp:Label ID="lblUsuario" CssClass="error-msj" runat="server" Visible="false"></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-contrasena">Contraseña</label>
          <input type="password" id="input-contrasena" name="contrasena">
          <asp:Label ID="lblContra" CssClass="error-msj" runat="server" Visible="false"></asp:Label>
      </div>

      <div class="input-container">
      <label for="input-confirmar-contra">Confirmar contraseña</label>
          <input type="password" id="input-confirmar-contra" name="confirmarContrasena">
         <asp:Label ID="lblConfirmar" CssClass="error-msj" runat="server" Visible="false"></asp:Label>
      </div>
      </div>
      </div>

      <span><a href="login.html">Ya tenes cuenta? Inicia sesión</a></span>

      <asp:Label ID="lblErrorGeneral" runat="server" CssClass="error-msj" Visible="false" />
      <input type="submit" class="btn-registrarse" value="REGISTRARSE">
        </div>
</asp:Content>
