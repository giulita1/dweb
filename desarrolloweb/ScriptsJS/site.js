console.log("hola");
const btnMenu = document.getElementById("btn-menu");
const navMenu = document.getElementById("nav-menu");

btnMenu.addEventListener("click", () => {

    navMenu.classList.toggle("active");

});
