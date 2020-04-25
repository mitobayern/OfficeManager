$(document).ready(function () {
    var createMeasurement = document.getElementById('createMeasurement');
    if (createMeasurement) {
        createMeasurement.querySelectorAll('input[type="text"]').forEach(function (el) {
            if (el.value === '0.00')
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