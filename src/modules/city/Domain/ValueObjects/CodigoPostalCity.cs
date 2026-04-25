// src/modules/city/Domain/ValueObjects/CodigoPostalCity.cs
namespace AirTicketSystem.modules.city.Domain.ValueObjects;

public sealed class CodigoPostalCity
{
    public string Valor { get; }

    private CodigoPostalCity(string valor) => Valor = valor;

    public static CodigoPostalCity Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código postal no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 3)
            throw new ArgumentException("El código postal debe tener al menos 3 caracteres.");

        if (normalizado.Length > 10)
            throw new ArgumentException("El código postal no puede superar 10 caracteres.");

        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"El código postal solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new CodigoPostalCity(normalizado);
    }

    public override string ToString() => Valor;
}