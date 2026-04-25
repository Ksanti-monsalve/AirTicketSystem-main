// src/modules/aircraftmodel/Domain/ValueObjects/NombreAircraftModel.cs
namespace AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

public sealed class NombreAircraftModel
{
    public string Valor { get; }

    private NombreAircraftModel(string valor) => Valor = valor;

    public static NombreAircraftModel Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del modelo de avión no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del modelo debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre del modelo no puede superar 100 caracteres.");

        return new NombreAircraftModel(normalizado);
    }

    public override string ToString() => Valor;
}