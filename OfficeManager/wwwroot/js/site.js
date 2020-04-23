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
    
    $('#deleteElectricityMeter').on('submit', function (ev) {
        if (!confirm('Are you sure you want to delete this Electricity meter?')) {
            ev.preventDefault();
            ev.stopPropagation();
        }
    });

    $('#deleteTemperatureMeter').on('submit', function (ev) {
        if (!confirm('Are you sure you want to delete this Temperature meter?')) {
            ev.preventDefault();
            ev.stopPropagation();
        }
    });

    $('#deleteOffice').on('submit', function (ev) {
        if (!confirm('Are you sure you want to delete this Office?')) {
            ev.preventDefault();
            ev.stopPropagation();
        }
    });

    $('#deleteTenant').on('submit', function (ev) {
        if (!confirm('Are you sure you want to delete this Tenant?')) {
            ev.preventDefault();
            ev.stopPropagation();
        }
    });

    $('#restartTenant').on('submit', function (ev) {
        if (!confirm('Are you sure you want to sign new contract with this Tenant?')) {
            ev.preventDefault();
            ev.stopPropagation();
        }
    });
});