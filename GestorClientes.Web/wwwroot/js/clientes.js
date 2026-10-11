// Manejo dinámico de las filas de teléfonos en los formularios de clientes.
(function () {
    const contenedor = document.getElementById('telefonos');
    const plantilla = document.getElementById('plantillaTelefono');
    const botonAgregar = document.getElementById('btnAgregarTelefono');

    if (!contenedor || !plantilla || !botonAgregar) {
        return;
    }

    // Renumera los índices (Telefonos[0], Telefonos[1], ...) para que el binding de MVC no tenga huecos.
    function reindexar() {
        contenedor.querySelectorAll('.fila-telefono').forEach(function (fila, indice) {
            fila.querySelectorAll('input, select').forEach(function (campo) {
                campo.name = campo.name.replace(/Telefonos\[[^\]]*\]/, 'Telefonos[' + indice + ']');
            });
        });
    }

    botonAgregar.addEventListener('click', function () {
        const fila = plantilla.content.firstElementChild.cloneNode(true);
        contenedor.appendChild(fila);
        reindexar();
        fila.querySelector('input').focus();
    });

    contenedor.addEventListener('click', function (evento) {
        const boton = evento.target.closest('.btn-quitar');

        if (boton) {
            boton.closest('.fila-telefono').remove();
            reindexar();
        }
    });

    contenedor.closest('form').addEventListener('submit', reindexar);
}());
