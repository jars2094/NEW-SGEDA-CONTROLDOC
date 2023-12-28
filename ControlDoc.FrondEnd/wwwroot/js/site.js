//Documentation https://www.daterangepicker.com/
window.initializeDateRangePicker = function (dotNetHelper) {
    $('input[name="daterange"]').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        dotNetHelper.invokeMethodAsync('HandleDateRangeSelection', start.format('YYYY/MM/DD'), end.format('YYYY/MM/DD'), label);
    });
};

//#region Dark-Mode

window.toggleTheme = () => {
    const body = document.body;
    body.classList.toggle('dark');

    const btnSwitch = document.querySelector('#switch');
    if (btnSwitch) {
        btnSwitch.classList.toggle('active');
    }

    //Almacenar el modo en Localstorage
    if (body.classList.contains('dark')) {
        localStorage.setItem('dark-theme', 'true');
    } else {
        localStorage.setItem('dark-theme', 'false');
    }
};

window.checkTheme = () => {
    if (localStorage.getItem('dark-theme') === 'true') {
        document.body.classList.add('dark');
    } else {
        document.body.classList.remove('dark');
    }
};

//#endregion

//Agregar Valores al Footer
window.changeFooter = (politicas, terminosyCondiciones) => {
    document.getElementById('Politicas').innerText = politicas;
    document.getElementById('TerminosyCondiciones').innerText = terminosyCondiciones;
};