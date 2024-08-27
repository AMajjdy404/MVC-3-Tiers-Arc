// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var Tempdata = document.querySelector(".Tempdata");

function removeTempdata() {
    if (Tempdata)
        Tempdata.remove();
}

setInterval(removeTempdata, 3000);