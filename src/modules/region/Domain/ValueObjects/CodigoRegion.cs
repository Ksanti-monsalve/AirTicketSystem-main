// src/modules/region/Domain/ValueObjects/CodigoRegion.cs
namespace AirTicketSystem.modules.region.Domain.ValueObjects;

public sealed class CodigoRegion
{
    public string Valor { get; }

    private CodigoRegion(string valor) => Valor = valor;

    public static CodigoRegion Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código de la región no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 2)
            throw new ArgumentException("El código de la región debe tener al menos 2 caracteres.");

        if (normalizado.Length > 10)
            throw new ArgumentException("El código de la región no puede superar 10 caracteres.");

        if (!normalizado.All(char.IsLetterOrDigit))
            throw new ArgumentException(
                $"El código de la región solo puede contener letras y números. Se recibió: '{valor}'");

        return new CodigoRegion(normalizado);
    }

    public override string ToString() => Valor;
}