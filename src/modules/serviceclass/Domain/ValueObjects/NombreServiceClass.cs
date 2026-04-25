// src/modules/serviceclass/Domain/ValueObjects/NombreServiceClass.cs
namespace AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

public sealed class NombreServiceClass
{
    public string Valor { get; }

    private NombreServiceClass(string valor) => Valor = valor;

    public static NombreServiceClass Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la clase de servicio no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre de la clase debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El nombre de la clase no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre de la clase de servicio no puede contener números.");

        return new NombreServiceClass(normalizado);
    }

    public override string ToString() => Valor;
}