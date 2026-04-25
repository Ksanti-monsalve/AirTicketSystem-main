// src/modules/fare/Domain/ValueObjects/NombreFare.cs
namespace AirTicketSystem.modules.fare.Domain.ValueObjects;

public sealed class NombreFare
{
    public string Valor { get; }

    private NombreFare(string valor) => Valor = valor;

    public static NombreFare Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la tarifa no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 3)
            throw new ArgumentException(
                "El nombre de la tarifa debe tener al menos 3 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException(
                "El nombre de la tarifa no puede superar 100 caracteres.");

        return new NombreFare(normalizado);
    }

    public override string ToString() => Valor;
}