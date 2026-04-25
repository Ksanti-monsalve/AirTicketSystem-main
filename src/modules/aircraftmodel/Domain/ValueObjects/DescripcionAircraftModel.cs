// src/modules/aircraftmodel/Domain/ValueObjects/DescripcionAircraftModel.cs
namespace AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

public sealed class DescripcionAircraftModel
{
    public string Valor { get; }

    private DescripcionAircraftModel(string valor) => Valor = valor;

    public static DescripcionAircraftModel Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La descripción del modelo no puede estar vacía.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException("La descripción debe tener al menos 5 caracteres.");

        if (normalizado.Length > 300)
            throw new ArgumentException("La descripción no puede superar 300 caracteres.");

        return new DescripcionAircraftModel(normalizado);
    }

    public override string ToString() => Valor;
}