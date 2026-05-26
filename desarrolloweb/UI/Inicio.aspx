<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="desarrolloweb.UI.Inicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <link rel="stylesheet"
        href="<%= ResolveUrl("~/Styles/inicio.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="hero">

     <img src="<%= ResolveUrl("~/img/principal.png") %>"
     alt="Hotel"
     class="imagen-principal">
    <div class="form-reservar">

        <div class="input-container">
        <label for="fecha-llegada">Fecha de llegada</label>
        <input type="date" id="fecha-llegada" class="input-date" name="fecha-llegada" required>
    </div>
    <div class="input-container">
        <label for="fecha-salida">Fecha de salida</label>
        <input type="date" id="fecha-salida" class="input-date" name="fecha-salida" required>
    </div>

    <button type="submit" class="btn-reservar">RESERVAR</button>
    </div>
        
</section>
   <section class="elhotel">

    <div class="descripcion-hotel">

        <span class="subtitulo-section">
            HOTEL 5 ESTRELLAS
        </span>

        <h1>El Hotel</h1>

        <p>
            Ubicado en el corazón de la Patagonia, nuestro hotel combina
            arquitectura elegante, naturaleza y experiencias exclusivas.
            Habitaciones premium, spa de lujo y vistas únicas acompañan
            cada estadía.
        </p>

        <div class="cards-hotel">

            <div class="card-hotel">
                <span class="material-symbols-outlined">bed</span>
                <h3>Suites Premium</h3>
                <p>Habitaciones amplias con vista panorámica.</p>
            </div>

            <div class="card-hotel">
                <span class="material-symbols-outlined">spa</span>
                <h3>Spa & Relax</h3>
                <p>Espacios diseñados para descanso total.</p>
            </div>

            <div class="card-hotel">
                <span class="material-symbols-outlined">pool</span>
                <h3>Piscina Climatizada</h3>
                <p>Disponible durante todo el año.</p>
            </div>

        </div>

    </div>

    <div class="imagenes-hotel">

        <div class="img-container">
            <img src="<%= ResolveUrl("~/img/hotel1.png") %>"
                 class="img-hotel">
        </div>

        <div class="img-container">
            <img src="<%= ResolveUrl("~/img/hotel2.png") %>"
                 class="img-hotel">
        </div>

    </div>

</section>

<section class="restaurante">

    <div class="img-rest-container">

        <img src="<%= ResolveUrl("~/img/restaurante.png") %>"
             alt="El restaurante"
             class="img-restaurante">

    </div>

    <div class="restaurante-descripcion">

        <span class="subtitulo-section">
            EXPERIENCIA GASTRONÓMICA
        </span>

        <h2>El Restaurante</h2>

        <p>
            Cocina de autor inspirada en sabores patagónicos,
            ingredientes frescos y una experiencia gastronómica
            acompañada por vinos seleccionados.
        </p>

        <div class="restaurant-info">

            <div class="info-item">
                <span class="material-symbols-outlined">restaurant</span>
                <p>Menú internacional</p>
            </div>

            <div class="info-item">
                <span class="material-symbols-outlined">wine_bar</span>
                <p>Cava premium</p>
            </div>

            <div class="info-item">
                <span class="material-symbols-outlined">schedule</span>
                <p>Abierto de 12hs a 00hs</p>
            </div>

        </div>

        <button type="button"
                id="btn-ver-restaurante"
                class="btn-ver">
            VER MENÚ
        </button>

    </div>

</section>
</asp:Content>
