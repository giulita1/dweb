<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="desarrolloweb.UI.Login" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Styles/login.css") %>" />
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

    <span><a href="<%= ResolveUrl("~/UI/Registro.aspx") %>" class="pregunta">¿No estás registrado? Registrate acá</a></span>
    <input type="submit" class="btn-login" value="INICIAR SESIÓN">

      </div>

  </div>


</asp:Content>