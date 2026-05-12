<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="desarrolloweb.UI.Registro" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet"
          href="<%= ResolveUrl("~/Styles/registro.css") %>" />

</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">    
    <form id="form-registro" class="form-registro" method="post" action="Registro.aspx">

      <div class="datos-container">
      <div class="datos-izquierda">
      <div class="input-container">
      <label for="input-nombre">Nombre</label>
          <input type="text" id="input-nombre" name="nombre">
      </div>

      <div class="input-container">
      <label for="input-apellido">Apellido</label>
          <input type="text" id="input-apellido" name="apellido">
      </div>

      <div class="input-container">
      <label for="input-email">Email</label>
          <input type="email" id="input-email" name="email">
      </div>
      </div>

      <div class="datos-derecha">
      <div class="input-container">
      <label for="input-usuario">Nombre de usuario</label>
          <input type="text" id="input-usuario" name="usuario">
      </div>

      <div class="input-container">
      <label for="input-contrasena">Contraseña</label>
          <input type="password" id="input-contrasena" name="contrasena">
      </div>

      <div class="input-container">
      <label for="confirmar-contrasena">Confirmar contraseña</label>
          <input type="text" id="input-confirmar-contra" name="confirmarContrasena">
      </div>
      </div>
      </div>

      <span><a href="login.html">Ya tenes cuenta? Inicia sesión</a></span>

      <input type="submit" class="btn-registrarse" value="REGISTRARSE">

  </form>
</asp:Content>
