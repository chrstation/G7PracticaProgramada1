namespace GestorClientes.Negocio;

/// <summary>
/// Excepción lanzada cuando una regla de negocio no se cumple.
/// </summary>
public class ValidacionException : Exception
{
    public ValidacionException(string mensaje)
        : base(mensaje)
    {
    }
}
