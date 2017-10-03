$(document).ready(function () {
    ShowPasswordLS();
    ShowTooltips();
});

function ShowPasswordLS() {
    $('.ShowPasswordLS').on('click', function () {
        var el = $(this);
        var input = $(el.attr("data-target"));
        el.toggleClass("active");
        if (el.hasClass("active")) {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
        input.focus();
    });
}

function ShowTooltips() {
    $('[data-toggle="tooltip"]').tooltip();
}