// src/modules/aircraftseat/Domain/ValueObjects/CodigoAsientoAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class CodigoAsientoAircraftSeat
{
    public string Valor { get; }
    public int Fila { get; }
    public char Columna { get; }

    private CodigoAsientoAircraftSeat(string valor, int fila, char columna)
    {
        Valor = valor;
        Fila = fila;
        Columna = columna;
    }

    public static CodigoAsientoAircraftSeat Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código de asiento no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 2 || normalizado.Length > 4)
            throw new ArgumentException(
                $"El código de asiento debe tener entre 2 y 4 caracteres. Se recibió: '{valor}'");

        var columna = normalizado[^1];

        if (!char.IsLetter(columna))
            throw new ArgumentException(
                $"La columna del asiento debe ser una letra. Se recibió: '{valor}'");

        if (columna < 'A' || columna > 'F')
            throw new ArgumentException(
                $"La columna del asiento debe estar entre A y F. Se recibió: '{columna}'");

        var filaParte = normalizado[..^1];

        if (!int.TryParse(filaParte, out var fila))
            throw new ArgumentException(
                $"La fila del asiento debe ser un número. Se recibió: '{valor}'");

        if (fila < 1 || fila > 99)
            throw new ArgumentException(
                $"La fila del asiento debe estar entre 1 y 99. Se recibió: '{fila}'");

        return new CodigoAsientoAircraftSeat(normalizado, fila, columna);
    }

    public static CodigoAsientoAircraftSeat Crear(int fila, char columna)
        => Crear($"{fila}{columna}");

    public override string ToString() => Valor;
}