<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="desarrolloweb.UI.Login" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/login.css") %>" />
    <script defer src="<%= ResolveUrl("~/ScriptsJS/login.js") %>"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

  <div id="form-login" class="form-login" >

      <div class="datos-container">
          <div class="input-container">
              <label for="input-usuario">Nombre de usuario</label>
              <input type="text" id="input-usuario" name="usuario">
          </div>

          <div class="input-container">
              <label for="input-contrasena">Contraseña</label>
              <input type="password" id="input-contrasena" name="contrasena">
          </div>

          <div style="display: flex; flex-direction: column; gap: 10px;">
              <span><a href="<%= ResolveUrl("~/UI/Registro.aspx") %>" class="pregunta">¿No estás registrado? Registrate acá</a></span>
              <span><a href="#" class="pregunta" onclick="toggleRecuperacion(event)">¿Olvidaste tu contraseña?</a></span>
          </div>

          <input type="submit" class="btn-login" value="INICIAR SESIÓN">
      </div>

      <div id="panel-recuperar" class="panel-recuperar">
          <hr class="separador" />
    
          <div class="input-container">
              <label for="txtEmailRecuperar">Ingresá tu Email registrado</label>
              <input type="email" id="txtEmailRecuperar" class="input-recuperar" placeholder="ejemplo@correo.com">
          </div>
    
          <button type="button" class="btn-login btn-recuperar" onclick="enviarClave()">
              ENVIAR NUEVA CLAVE
          </button>
      </div>

  </div>
</asp:Content>