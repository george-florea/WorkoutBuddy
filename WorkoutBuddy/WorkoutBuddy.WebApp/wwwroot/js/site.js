// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(".sidemenu").click(e => {
    if ($(".sidebar").data("clicked") == "false") {
        $(".sidebar").css("display", "block");
        $(".sidebar").data("clicked", "true")
        $(".page-content").css("padding-left", "250px");
    }
    else {
        $(".sidebar").css("display", "none");
        $(".sidebar").data("clicked", "false");
        $(".page-content").css("padding-left", "0");
    }
});