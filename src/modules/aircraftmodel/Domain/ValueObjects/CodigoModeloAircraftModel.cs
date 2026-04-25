// src/modules/aircraftmodel/Domain/ValueObjects/CodigoModeloAircraftModel.cs
namespace AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

public sealed class CodigoModeloAircraftModel
{
    public string Valor { get; }

    private CodigoModeloAircraftModel(string valor) => Valor = valor;

    public static CodigoModeloAircraftModel Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código del modelo no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 2)
            throw new ArgumentException("El código del modelo debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El código del modelo no puede superar 50 caracteres.");

        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"El código del modelo solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new CodigoModeloAircraftModel(normalizado);
    }

    public override string ToString() => Valor;
}