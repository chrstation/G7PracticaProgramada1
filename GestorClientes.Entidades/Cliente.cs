namespace GestorClientes.Entidades;

/// <summary>
/// Representa un cliente del negocio.
/// </summary>
public class Cliente
{
    public int Id { get; set; }

    public string Cedula { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public List<Telefono> Telefonos { get; set; } = new();

    public string NombreCompleto => $"{Nombre} {Apellido}";
}
