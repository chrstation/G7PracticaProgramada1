using GestorClientes.Entidades;
using GestorClientes.Negocio;

namespace GestorClientes.Consola;

/// <summary>
/// Capa de presentación: menú interactivo de consola para gestionar clientes.
/// </summary>
public class MenuClientes
{
    private readonly IClienteServicio _servicio;

    public MenuClientes(IClienteServicio servicio)
    {
        _servicio = servicio;
    }

    public async Task EjecutarAsync()
    {
        var salir = false;

        while (!salir)
        {
            MostrarMenu();
            var opcion = Console.ReadLine()?.Trim();

            try
            {
                switch (opcion)
                {
                    case "1":
                        await ListarClientesAsync();
                        break;
                    case "2":
                        await VerDetalleAsync();
                        break;
                    case "3":
                        await RegistrarAsync();
                        break;
                    case "4":
                        await ModificarAsync();
                        break;
                    case "5":
                        await BorrarAsync();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (ValidacionException ex)
            {
                Console.WriteLine($"\n[Error] {ex.Message}");
            }

            if (!salir)
            {
                Console.WriteLine("\nPresione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    private static void MostrarMenu()
    {
        Console.Clear();
        Console.WriteLine("=== GESTOR DE CLIENTES ===");
        Console.WriteLine("1. Listado de clientes");
        Console.WriteLine("2. Ver detalle de un cliente");
        Console.WriteLine("3. Registrar cliente");
        Console.WriteLine("4. Modificar cliente");
        Console.WriteLine("5. Borrar cliente");
        Console.WriteLine("0. Salir");
        Console.Write("\nSeleccione una opción: ");
    }

    private async Task ListarClientesAsync()
    {
        var clientes = await _servicio.ListarAsync();

        Console.WriteLine("\n--- Listado de clientes ---");

        if (clientes.Count == 0)
        {
            Console.WriteLine("No hay clientes registrados.");
            return;
        }

        foreach (var cliente in clientes)
        {
            var telefonos = cliente.Telefonos.Count > 0
                ? string.Join(", ", cliente.Telefonos.Select(t => t.Numero))
                : "Sin teléfonos";

            Console.WriteLine($"[{cliente.Id}] {cliente.NombreCompleto} | Cédula: {cliente.Cedula} | Tel: {telefonos}");
        }
    }

    private async Task VerDetalleAsync()
    {
        var id = LeerEntero("\nId del cliente: ");
        var cliente = await _servicio.ObtenerDetalleAsync(id);

        Console.WriteLine("\n--- Detalle del cliente ---");
        Console.WriteLine($"Id:             {cliente.Id}");
        Console.WriteLine($"Cédula:         {cliente.Cedula}");
        Console.WriteLine($"Nombre:         {cliente.NombreCompleto}");
        Console.WriteLine($"Correo:         {cliente.Correo}");
        Console.WriteLine($"Dirección:      {cliente.Direccion}");
        Console.WriteLine($"Fecha registro: {cliente.FechaRegistro:dd/MM/yyyy HH:mm}");
        Console.WriteLine("Teléfonos:");

        foreach (var telefono in cliente.Telefonos)
        {
            Console.WriteLine($"  - {telefono.Numero} ({telefono.Tipo})");
        }
    }

    private async Task RegistrarAsync()
    {
        Console.WriteLine("\n--- Registrar cliente ---");
        var cliente = new Cliente();
        LeerDatos(cliente);

        await _servicio.RegistrarAsync(cliente);
        Console.WriteLine("\nCliente registrado correctamente.");
    }

    private async Task ModificarAsync()
    {
        var id = LeerEntero("\nId del cliente a modificar: ");
        var cliente = await _servicio.ObtenerDetalleAsync(id);

        Console.WriteLine("\n--- Modificar cliente (deje vacío para mantener el valor actual) ---");
        LeerDatos(cliente, esModificacion: true);

        await _servicio.ModificarAsync(cliente);
        Console.WriteLine("\nCliente modificado correctamente.");
    }

    private async Task BorrarAsync()
    {
        var id = LeerEntero("\nId del cliente a borrar: ");
        var cliente = await _servicio.ObtenerDetalleAsync(id);

        Console.Write($"¿Borrar a {cliente.NombreCompleto} y sus teléfonos? (s/n): ");

        if (Console.ReadLine()?.Trim().ToLower() != "s")
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        await _servicio.BorrarAsync(id);
        Console.WriteLine("\nCliente borrado correctamente.");
    }

    private static void LeerDatos(Cliente cliente, bool esModificacion = false)
    {
        cliente.Cedula = LeerTexto("Cédula", cliente.Cedula, esModificacion);
        cliente.Nombre = LeerTexto("Nombre", cliente.Nombre, esModificacion);
        cliente.Apellido = LeerTexto("Apellido", cliente.Apellido, esModificacion);
        cliente.Correo = LeerTexto("Correo", cliente.Correo, esModificacion);
        cliente.Direccion = LeerTexto("Dirección", cliente.Direccion, esModificacion);

        if (esModificacion)
        {
            Console.Write("¿Reemplazar los teléfonos actuales? (s/n): ");

            if (Console.ReadLine()?.Trim().ToLower() != "s")
            {
                return;
            }
        }

        cliente.Telefonos = new List<Telefono>();
        Console.WriteLine("Teléfonos (deje el número vacío para terminar):");

        while (true)
        {
            Console.Write("  Número: ");
            var numero = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(numero))
            {
                break;
            }

            Console.Write("  Tipo (Móvil/Casa/Trabajo): ");
            var tipo = Console.ReadLine()?.Trim();

            cliente.Telefonos.Add(new Telefono
            {
                Numero = numero,
                Tipo = string.IsNullOrEmpty(tipo) ? "Móvil" : tipo
            });
        }
    }

    private static string LeerTexto(string etiqueta, string valorActual, bool esModificacion)
    {
        Console.Write(esModificacion ? $"{etiqueta} [{valorActual}]: " : $"{etiqueta}: ");
        var entrada = Console.ReadLine()?.Trim();

        return string.IsNullOrEmpty(entrada) && esModificacion ? valorActual : entrada ?? string.Empty;
    }

    private static int LeerEntero(string mensaje)
    {
        Console.Write(mensaje);

        if (!int.TryParse(Console.ReadLine(), out var valor))
        {
            throw new ValidacionException("Debe ingresar un número entero válido.");
        }

        return valor;
    }
}
