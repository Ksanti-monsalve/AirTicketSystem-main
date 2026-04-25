// src/modules/serviceclass/Domain/ValueObjects/CodigoServiceClass.cs
namespace AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

public sealed class CodigoServiceClass
{
    public string Valor { get; }

    private CodigoServiceClass(string valor) => Valor = valor;

    public static CodigoServiceClass Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código de la clase de servicio no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 3)
            throw new ArgumentException(
                $"El código de la clase de servicio debe tener exactamente 3 letras. Se recibió: '{valor}'");

        if (!normalizado.All(char.IsLetter))
            throw new ArgumentException(
                $"El código de la clase solo puede contener letras. Se recibió: '{valor}'");

        return new CodigoServiceClass(normalizado);
    }

    public override string ToString() => Valor;
}