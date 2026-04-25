// src/modules/luggage/Domain/ValueObjects/DescripcionLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class DescripcionLuggage
{
    public string Valor { get; }

    private DescripcionLuggage(string valor) => Valor = valor;

    public static DescripcionLuggage Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción del equipaje no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 3)
            throw new ArgumentException(
                "La descripción del equipaje debe tener al menos 3 caracteres.");

        if (normalizado.Length > 200)
            throw new ArgumentException(
                "La descripción del equipaje no puede superar 200 caracteres.");

        return new DescripcionLuggage(normalizado);
    }

    public override string ToString() => Valor;
}