namespace GestorClientes.Entidades;

/// <summary>
/// Número de teléfono asociado a un cliente.
/// </summary>
public class Telefono
{
    public int Id { get; set; }

    public string Numero { get; set; } = string.Empty;

    public string Tipo { get; set; } = "Móvil";

    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }
}
