// src/modules/seatavailability/Domain/ValueObjects/EstadoSeatAvailability.cs
namespace AirTicketSystem.modules.seatavailability.Domain.ValueObjects;

public sealed class EstadoSeatAvailability
{
    private static readonly string[] _estadosValidos =
    {
        "DISPONIBLE",
        "RESERVADO",
        "OCUPADO",
        "BLOQUEADO"
    };

    public string Valor { get; }

    private EstadoSeatAvailability(string valor) => Valor = valor;

    public static EstadoSeatAvailability Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del asiento no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoSeatAvailability(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoSeatAvailability Disponible() => new("DISPONIBLE");
    public static EstadoSeatAvailability Reservado() => new("RESERVADO");
    public static EstadoSeatAvailability Ocupado() => new("OCUPADO");
    public static EstadoSeatAvailability Bloqueado() => new("BLOQUEADO");

    // Propiedades de negocio
    public bool PuedeReservarse => Valor == "DISPONIBLE";

    public bool EstaOcupado =>
        Valor == "RESERVADO" ||
        Valor == "OCUPADO";

    // Transiciones válidas
    public bool PuedeTransicionarA(EstadoSeatAvailability nuevoEstado)
    {
        return Valor switch
        {
            "DISPONIBLE" => nuevoEstado.Valor is "RESERVADO" or "BLOQUEADO",
            "RESERVADO"  => nuevoEstado.Valor is "OCUPADO" or "DISPONIBLE",
            "OCUPADO"    => false,
            "BLOQUEADO"  => nuevoEstado.Valor is "DISPONIBLE",
            _            => false
        };
    }

    public override string ToString() => Valor;
}