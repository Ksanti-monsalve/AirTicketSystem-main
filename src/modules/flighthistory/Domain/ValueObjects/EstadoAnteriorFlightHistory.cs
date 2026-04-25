// src/modules/flighthistory/Domain/ValueObjects/EstadoAnteriorFlightHistory.cs
namespace AirTicketSystem.modules.flighthistory.Domain.ValueObjects;

public sealed class EstadoAnteriorFlightHistory
{
    private static readonly string[] _estadosValidos =
    {
        "PROGRAMADO", "ABORDANDO", "EN_VUELO",
        "ATERRIZADO", "CANCELADO", "DEMORADO", "DESVIADO"
    };

    public string Valor { get; }

    private EstadoAnteriorFlightHistory(string valor) => Valor = valor;

    public static EstadoAnteriorFlightHistory Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado anterior del vuelo no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado anterior '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoAnteriorFlightHistory(normalizado);
    }

    public override string ToString() => Valor;
}