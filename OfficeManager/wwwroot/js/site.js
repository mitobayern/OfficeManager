$(document).ready(function () {
    $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');
        $(this).toggleClass('show');

        return false;
    });

    $('#dropdown').on('hidden.bs.dropdown', function () {
        $('#dropdown .dropdown-submenu .show').removeClass('show');
        $('#dropdown a.dropdown-toggle').removeClass('show');
    });

    const createMeasurement = document.getElementById('createMeasurement');
    if (createMeasurement) {
        createMeasurement.querySelectorAll('input[type="text"]').forEach(function (el) {
            el.value = ''
        });
    }
});