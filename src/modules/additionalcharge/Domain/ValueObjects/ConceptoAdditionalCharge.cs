// src/modules/additionalcharge/Domain/ValueObjects/ConceptoAdditionalCharge.cs
namespace AirTicketSystem.modules.additionalcharge.Domain.ValueObjects;

public sealed class ConceptoAdditionalCharge
{
    public string Valor { get; }

    private ConceptoAdditionalCharge(string valor) => Valor = valor;

    public static ConceptoAdditionalCharge Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El concepto del cargo adicional no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 3)
            throw new ArgumentException(
                "El concepto del cargo debe tener al menos 3 caracteres.");

        if (normalizado.Length > 150)
            throw new ArgumentException(
                "El concepto del cargo no puede superar 150 caracteres.");

        return new ConceptoAdditionalCharge(normalizado);
    }

    public override string ToString() => Valor;
}