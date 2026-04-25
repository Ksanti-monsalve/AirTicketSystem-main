// src/modules/flight/Domain/ValueObjects/NumeroVueloFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class NumeroVueloFlight
{
    public string Valor { get; }
    public string CodigoAerolinea { get; }
    public string Numero { get; }

    private NumeroVueloFlight(string valor, string codigoAerolinea, string numero)
    {
        Valor = valor;
        CodigoAerolinea = codigoAerolinea;
        Numero = numero;
    }

    public static NumeroVueloFlight Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El número de vuelo no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant().Replace(" ", "");

        if (normalizado.Length < 3 || normalizado.Length > 6)
            throw new ArgumentException(
                $"El número de vuelo debe tener entre 3 y 6 caracteres. Se recibió: '{valor}'");

        // Primeros 2 caracteres: código IATA de aerolínea (letras)
        var codigoAerolinea = normalizado[..2];
        if (!codigoAerolinea.All(char.IsLetter))
            throw new ArgumentException(
                $"Los primeros 2 caracteres del vuelo deben ser el código de aerolínea. Se recibió: '{valor}'");

        // Resto: número de vuelo (dígitos)
        var numero = normalizado[2..];
        if (!numero.All(char.IsDigit))
            throw new ArgumentException(
                $"Los últimos caracteres del número de vuelo deben ser dígitos. Se recibió: '{valor}'");

        if (numero.Length < 1 || numero.Length > 4)
            throw new ArgumentException(
                $"El número de vuelo debe tener entre 1 y 4 dígitos después del código. Se recibió: '{valor}'");

        return new NumeroVueloFlight(normalizado, codigoAerolinea, numero);
    }

    public override string ToString() => Valor;
}