using System.ComponentModel.DataAnnotations;
using GestorClientes.Entidades;

namespace GestorClientes.Web.Models;

/// <summary>
/// Datos de un cliente para las vistas. Evita exponer directamente la entidad del dominio.
/// </summary>
public class ClienteViewModel
{
    public int Id { get; set; }

    [Display(Name = "Cédula")]
    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [StringLength(20, ErrorMessage = "La cédula no puede superar los 20 caracteres.")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
    public string? Correo { get; set; }

    [Display(Name = "Dirección")]
    [StringLength(250, ErrorMessage = "La dirección no puede superar los 250 caracteres.")]
    public string? Direccion { get; set; }

    [Display(Name = "Fecha de registro")]
    public DateTime FechaRegistro { get; set; }

    [Display(Name = "Teléfonos")]
    public List<TelefonoViewModel> Telefonos { get; set; } = new();

    public string NombreCompleto => $"{Nombre} {Apellido}";

    public string TelefonosTexto => Telefonos.Count > 0
        ? string.Join(", ", Telefonos.Select(t => t.Numero))
        : "Sin teléfonos";

    public static ClienteViewModel DesdeEntidad(Cliente cliente)
    {
        return new ClienteViewModel
        {
            Id = cliente.Id,
            Cedula = cliente.Cedula,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Correo = cliente.Correo,
            Direccion = cliente.Direccion,
            FechaRegistro = cliente.FechaRegistro,
            Telefonos = cliente.Telefonos
                .Select(t => new TelefonoViewModel { Numero = t.Numero, Tipo = t.Tipo })
                .ToList()
        };
    }

    public Cliente ComoEntidad()
    {
        return new Cliente
        {
            Id = Id,
            Cedula = Cedula.Trim(),
            Nombre = Nombre.Trim(),
            Apellido = Apellido.Trim(),
            Correo = Correo?.Trim() ?? string.Empty,
            Direccion = Direccion?.Trim() ?? string.Empty,
            // Se ignoran las filas de teléfono que quedaron vacías.
            Telefonos = Telefonos
                .Where(t => !string.IsNullOrWhiteSpace(t.Numero))
                .Select(t => new Telefono
                {
                    Numero = t.Numero!.Trim(),
                    Tipo = string.IsNullOrWhiteSpace(t.Tipo) ? "Móvil" : t.Tipo
                })
                .ToList()
        };
    }
}
