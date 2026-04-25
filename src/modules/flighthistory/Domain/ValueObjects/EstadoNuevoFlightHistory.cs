// src/modules/flighthistory/Domain/ValueObjects/EstadoNuevoFlightHistory.cs
namespace AirTicketSystem.modules.flighthistory.Domain.ValueObjects;

public sealed class EstadoNuevoFlightHistory
{
    private static readonly string[] _estadosValidos =
    {
        "PROGRAMADO", "ABORDANDO", "EN_VUELO",
        "ATERRIZADO", "CANCELADO", "DEMORADO", "DESVIADO"
    };

    public string Valor { get; }

    private EstadoNuevoFlightHistory(string valor) => Valor = valor;

    public static EstadoNuevoFlightHistory Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado nuevo del vuelo no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado nuevo '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoNuevoFlightHistory(normalizado);
    }

    public override string ToString() => Valor;
}