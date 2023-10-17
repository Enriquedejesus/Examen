


function actualizarTablaDispositivos() {
    $.get('/Dispositivo/GetAllDispositivos', function (data) {
        console.log(data);
        const tbody = $("table tbody");
        tbody.empty(); // Limpia el cuerpo de la tabla

        data.forEach(d => {
            // Convierte el valor numérico de estatus a texto
            const estatusTexto = d.estatus === 1 ? "Encendido" : "Apagado";

            const row = `
                <tr>
                    <td>${d.id}</td>
                    <td>${d.nombre}</td>
                    <td>${d.latitud}</td>
                    <td>${d.longitud}</td>
                    <td>${d.descripcion}</td>
                    <td>${d.prioridad}</td>
                    <td>${estatusTexto}</td>
                    <td>${d.litrosRegistrados}</td>
                    <td>${d.fechaCreacion}</td>
                    <td> ${d.responsable} </td>
                     <td> ${d.zona} </td>
                    <td>
                        <form action="/Dispositivo/Delete/${d.id}" method="post" style="display:inline">
                            <input type="submit" value="Eliminar" class="btn btn-danger btn-sm" 
                            onclick="return confirm('¿Estás seguro de que deseas eliminar este dispositivo?');" />
                        </form>
                        <form method="post" action="/Dispositivo/CambiarEstado" style="display:inline">
                            <input type="hidden" name="id" value="${d.id}" />
                            <input type="submit" value="Cambiar Estado" class="btn btn-warning btn-sm" />
                        </form>
                    </td>
                </tr>
            `;
            tbody.append(row);
        });
    });
}


//GRAFICAS 

var litrosTotalChart;

$.getJSON('/Dispositivo/GetTotalLitros', function (data) {
    var ctxLitrosTotal = document.getElementById('litrosTotalChart').getContext('2d');
    litrosTotalChart = new Chart(ctxLitrosTotal, {
        type: 'bar',
        data: {
            labels: ["Litros Totales"], // Esto es para que solo se muestre una barra con el total
            datasets: [{
                label: 'Litros Totales',
                data: [data.totalLitros],
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
});


// Escuchadores 

function actualizarGraficaEstatus() {
    $.ajax({
        url: '/Dispositivo/ObtenerDatosGraficaEstatus',
        method: 'GET',
        success: function (data) {
            estatusChart.data.datasets[0].data = [data.dispositivosApagados, data.dispositivosEncendidos];
            estatusChart.update();
        }
    });
} // <-- Cierra aquí

function actualizarGraficaLitros() {
    $.ajax({
        url: '/Dispositivo/ObtenerDatosGraficaLitros',
        method: 'GET',
        success: function (data) {
            litrosChart.data.labels = data.nombres;
            litrosChart.data.datasets[0].data = data.litrosPorDispositivo;
            litrosChart.update();
        }

    });
} // <-- Cierra aquí

function actualizarGraficaLitrosTotales() {
    $.ajax({
        url: '/Dispositivo/GetTotalLitros',
        method: 'GET',
        success: function (data) {

            litrosTotalChart.data.datasets[0].data = [data.totalLitros];  // Aquí adaptamos el acceso a la propiedad
            litrosTotalChart.update();
        }
    });
}



function MostrarNotificaciones(msj) {
    if (!("Notification" in window)) {
        alert("El navegador no soporta notificaciones");
    } else if (Notification.permission === "granted") {
        const noti = new Notification(msj);
    } else if (Notification.permission !== "denied") {
        Notification.requestPermission(function (permission) {
            if (permission === "granted") {
                const noti = new Notification("Permiso autorizado");
            }
        });
    }
}
var markersGroup = L.layerGroup().addTo(map);

function actualizarMarcadores() {

    /*  var markersGroup = L.layerGroup().addTo(map);    */

    $.ajax({
        url: '/Dispositivo/GetAllDispositivos',
        method: 'GET',
        cache: false,
        success: function (data) {
            console.log("Dispositivos recibidos del servidor:", data);
            // Eliminar los marcadores actuales
            markersGroup.clearLayers(); // primer uso

            // Agregar los nuevos marcadores
            data.forEach(function (dispositivo) {
                L.marker([dispositivo.latitud, dispositivo.longitud]).addTo(markersGroup)
                    .bindPopup('Dispositivo : ' + dispositivo.nombre);
            });
        }
    });
}


const contenedor = document.getElementById("contenedor");
const conexion = new signalR.HubConnectionBuilder()
    .withUrl("/myHub")
    .build();
conexion.start().then(function () {
    console.log("conexion exitosa");
}).catch((error) => {
    console.error("Error al conectarse al servidor de WebSocket", error);
});

conexion.on("DispositivoEliminado", function (nombreDispositivo, fechaHora) {
    console.log("Evento DispositivoEliminado recibido");
    MostrarNotificaciones(`Nombre: ${nombreDispositivo} Eliminado el ${fechaHora}`);
    actualizarTablaDispositivos();
    actualizarGraficaEstatus();
    actualizarGraficaLitros();
    actualizarGraficaLitrosTotales();
    actualizarMarcadores();
});



conexion.on("DispositivoAgregado", function (mensaje) {
    console.log("Evento DispositivoAgregado recibido");
    MostrarNotificaciones(mensaje);  // Esta función mostraría una notificación al usuario
    actualizarTablaDispositivos();  // Esta función actualiza la tabla
    actualizarGraficaEstatus();
    actualizarGraficaLitros();
    actualizarGraficaLitrosTotales();
    actualizarMarcadores();
});
conexion.on("EstatusCambiado", function (nombreDelDispositivo, nuevoEstatus) {
    MostrarNotificaciones(`Nombre: ${nombreDelDispositivo} ha cambiado su estado a: ${nuevoEstatus}`);
    actualizarGraficaEstatus();

});
conexion.on("ActualizarTablaDispositivos", function () {
    actualizarTablaDispositivos(); // Esta es la función que ya tenías definida para cargar la tabla con AJAX
});

conexion.on("TodosApagados", function (mensaje) {
    console.log("Evento TodosApagados recibido:", mensaje);
    MostrarNotificaciones(mensaje);
});

/* 
$.connection.hub.start().done(function () {


    
   
    
    
});
 */



