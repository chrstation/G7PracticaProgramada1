using GestorClientes.Entidades;

namespace GestorClientes.Negocio;

/// <summary>
/// Contrato de la lógica de negocio para la gestión de clientes.
/// </summary>
public interface IClienteServicio
{
    Task<List<Cliente>> ListarAsync();

    Task<Cliente> ObtenerDetalleAsync(int id);

    Task RegistrarAsync(Cliente cliente);

    Task ModificarAsync(Cliente cliente);

    Task BorrarAsync(int id);
}
