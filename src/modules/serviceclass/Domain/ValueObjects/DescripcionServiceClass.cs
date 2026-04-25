// src/modules/serviceclass/Domain/ValueObjects/DescripcionServiceClass.cs
namespace AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

public sealed class DescripcionServiceClass
{
    public string Valor { get; }

    private DescripcionServiceClass(string valor) => Valor = valor;

    public static DescripcionServiceClass Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción de la clase de servicio no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException("La descripción debe tener al menos 5 caracteres.");

        if (normalizado.Length > 200)
            throw new ArgumentException("La descripción no puede superar 200 caracteres.");

        return new DescripcionServiceClass(normalizado);
    }

    public override string ToString() => Valor;
}