using GestorClientes.Entidades;

namespace GestorClientes.Datos;

/// <summary>
/// Contrato de acceso a datos para clientes (patrón Repository).
/// </summary>
public interface IClienteRepositorio
{
    Task<List<Cliente>> ObtenerTodosAsync();

    Task<Cliente?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteCedulaAsync(string cedula, int? idExcluido = null);

    Task AgregarAsync(Cliente cliente);

    Task<bool> ActualizarAsync(Cliente cliente);

    Task<bool> EliminarAsync(int id);
}
