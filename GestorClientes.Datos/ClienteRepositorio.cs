using GestorClientes.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestorClientes.Datos;

/// <summary>
/// Implementación del repositorio de clientes usando Entity Framework Core.
/// </summary>
public class ClienteRepositorio : IClienteRepositorio
{
    private readonly AppDbContext _contexto;

    public ClienteRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Cliente>> ObtenerTodosAsync()
    {
        return await _contexto.Clientes
            .Include(c => c.Telefonos)
            .AsNoTracking()
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Clientes
            .Include(c => c.Telefonos)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExisteCedulaAsync(string cedula, int? idExcluido = null)
    {
        return await _contexto.Clientes
            .AnyAsync(c => c.Cedula == cedula && c.Id != idExcluido);
    }

    public async Task AgregarAsync(Cliente cliente)
    {
        _contexto.Clientes.Add(cliente);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(Cliente cliente)
    {
        var existente = await _contexto.Clientes
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.Id == cliente.Id);

        if (existente is null)
        {
            return false;
        }

        existente.Cedula = cliente.Cedula;
        existente.Nombre = cliente.Nombre;
        existente.Apellido = cliente.Apellido;
        existente.Correo = cliente.Correo;
        existente.Direccion = cliente.Direccion;

        // Se reemplaza la lista de teléfonos por la nueva.
        _contexto.Telefonos.RemoveRange(existente.Telefonos);
        existente.Telefonos = cliente.Telefonos
            .Select(t => new Telefono { Numero = t.Numero, Tipo = t.Tipo })
            .ToList();

        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var existente = await _contexto.Clientes
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existente is null)
        {
            return false;
        }

        _contexto.Clientes.Remove(existente);
        await _contexto.SaveChangesAsync();
        return true;
    }
}
