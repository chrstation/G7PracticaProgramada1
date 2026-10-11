using System.ComponentModel.DataAnnotations;

namespace GestorClientes.Web.Models;

/// <summary>
/// Datos de un teléfono mostrados y recibidos en los formularios.
/// </summary>
public class TelefonoViewModel
{
    [RegularExpression(@"^[0-9+\- ]+$", ErrorMessage = "El teléfono solo puede contener números, +, - y espacios.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    public string? Numero { get; set; }

    public string? Tipo { get; set; } = "Móvil";
}
