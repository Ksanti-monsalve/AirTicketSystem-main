// src/modules/bookingpassenger/Domain/ValueObjects/TipoPasajeroBookingPassenger.cs
namespace AirTicketSystem.modules.bookingpassenger.Domain.ValueObjects;

public sealed class TipoPasajeroBookingPassenger
{
    private static readonly string[] _tiposValidos =
    {
        "ADULTO",
        "MENOR",
        "INFANTE"
    };

    public string Valor { get; }

    private TipoPasajeroBookingPassenger(string valor) => Valor = valor;

    public static TipoPasajeroBookingPassenger Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El tipo de pasajero no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_tiposValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El tipo de pasajero '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _tiposValidos)}");

        return new TipoPasajeroBookingPassenger(normalizado);
    }

    // Métodos de fábrica expresivos
    public static TipoPasajeroBookingPassenger Adulto() => new("ADULTO");
    public static TipoPasajeroBookingPassenger Menor() => new("MENOR");
    public static TipoPasajeroBookingPassenger Infante() => new("INFANTE");

    // Propiedades de negocio
    public bool RequiereAsientoPropio =>
        Valor == "ADULTO" || Valor == "MENOR";

    public bool ViajaEnFalda =>
        Valor == "INFANTE";

    public bool RequiereAdultoAcompanante =>
        Valor == "MENOR" || Valor == "INFANTE";

    /// <summary>
    /// Determina el tipo de pasajero según la edad.
    /// Regla estándar IATA.
    /// </summary>
    public static TipoPasajeroBookingPassenger DeterminarPorEdad(int edadAnos)
    {
        if (edadAnos < 0)
            throw new ArgumentException("La edad no puede ser negativa.");

        return edadAnos switch
        {
            < 2  => Infante(),
            < 12 => Menor(),
            _    => Adulto()
        };
    }

    public override string ToString() => Valor;
}