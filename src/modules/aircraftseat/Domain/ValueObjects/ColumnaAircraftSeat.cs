// src/modules/aircraftseat/Domain/ValueObjects/ColumnaAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class ColumnaAircraftSeat
{
    public char Valor { get; }

    private ColumnaAircraftSeat(char valor) => Valor = valor;

    public static ColumnaAircraftSeat Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La columna del asiento no puede estar vacía.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length != 1)
            throw new ArgumentException(
                $"La columna del asiento debe ser exactamente 1 letra. Se recibió: '{valor}'");

        var columna = normalizado[0];

        if (!char.IsLetter(columna))
            throw new ArgumentException(
                $"La columna del asiento debe ser una letra. Se recibió: '{valor}'");

        if (columna < 'A' || columna > 'F')
            throw new ArgumentException(
                $"La columna del asiento debe estar entre A y F. Se recibió: '{columna}'");

        return new ColumnaAircraftSeat(columna);
    }

    public static ColumnaAircraftSeat Crear(char valor)
        => Crear(valor.ToString());

    public override string ToString() => Valor.ToString();
}