using System.Net.Mail;
using GestorClientes.Datos;
using GestorClientes.Entidades;

namespace GestorClientes.Negocio;

/// <summary>
/// Implementa las reglas de negocio de los clientes.
/// Depende de la abstracción IClienteRepositorio (inversión de dependencias).
/// </summary>
public class ClienteServicio : IClienteServicio
{
    private readonly IClienteRepositorio _repositorio;

    public ClienteServicio(IClienteRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<List<Cliente>> ListarAsync()
    {
        return _repositorio.ObtenerTodosAsync();
    }

    public async Task<Cliente> ObtenerDetalleAsync(int id)
    {
        var cliente = await _repositorio.ObtenerPorIdAsync(id);

        return cliente ?? throw new ValidacionException($"No existe un cliente con el Id {id}.");
    }

    public async Task RegistrarAsync(Cliente cliente)
    {
        Validar(cliente);

        if (await _repositorio.ExisteCedulaAsync(cliente.Cedula))
        {
            throw new ValidacionException("Ya existe un cliente con esa cédula.");
        }

        cliente.FechaRegistro = DateTime.Now;
        await _repositorio.AgregarAsync(cliente);
    }

    public async Task ModificarAsync(Cliente cliente)
    {
        Validar(cliente);

        if (await _repositorio.ExisteCedulaAsync(cliente.Cedula, cliente.Id))
        {
            throw new ValidacionException("Otro cliente ya utiliza esa cédula.");
        }

        var actualizado = await _repositorio.ActualizarAsync(cliente);

        if (!actualizado)
        {
            throw new ValidacionException($"No existe un cliente con el Id {cliente.Id}.");
        }
    }

    public async Task BorrarAsync(int id)
    {
        var eliminado = await _repositorio.EliminarAsync(id);

        if (!eliminado)
        {
            throw new ValidacionException($"No existe un cliente con el Id {id}.");
        }
    }

    private static void Validar(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Cedula))
        {
            throw new ValidacionException("La cédula es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(cliente.Nombre))
        {
            throw new ValidacionException("El nombre es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(cliente.Apellido))
        {
            throw new ValidacionException("El apellido es obligatorio.");
        }

        if (!string.IsNullOrWhiteSpace(cliente.Correo) && !CorreoValido(cliente.Correo))
        {
            throw new ValidacionException("El correo electrónico no tiene un formato válido.");
        }

        foreach (var telefono in cliente.Telefonos)
        {
            if (string.IsNullOrWhiteSpace(telefono.Numero) || !telefono.Numero.All(c => char.IsDigit(c) || c is '+' or '-' or ' '))
            {
                throw new ValidacionException($"El teléfono '{telefono.Numero}' no es válido.");
            }
        }
    }

    private static bool CorreoValido(string correo)
    {
        return MailAddress.TryCreate(correo, out _);
    }
}
