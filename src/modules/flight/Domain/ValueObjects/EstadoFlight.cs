// src/modules/flight/Domain/ValueObjects/EstadoFlight.cs
namespace AirTicketSystem.modules.flight.Domain.ValueObjects;

public sealed class EstadoFlight
{
    private static readonly string[] _estadosValidos =
    {
        "PROGRAMADO",
        "ABORDANDO",
        "EN_VUELO",
        "ATERRIZADO",
        "CANCELADO",
        "DEMORADO",
        "DESVIADO"
    };

    public string Valor { get; }

    private EstadoFlight(string valor) => Valor = valor;

    public static EstadoFlight Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del vuelo no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoFlight(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoFlight Programado() => new("PROGRAMADO");
    public static EstadoFlight Abordando() => new("ABORDANDO");
    public static EstadoFlight EnVuelo() => new("EN_VUELO");
    public static EstadoFlight Aterrizado() => new("ATERRIZADO");
    public static EstadoFlight Cancelado() => new("CANCELADO");
    public static EstadoFlight Demorado() => new("DEMORADO");
    public static EstadoFlight Desviado() => new("DESVIADO");

    // Propiedades de negocio — determinan qué operaciones son válidas
    public bool PermiteCheckin =>
        Valor == "PROGRAMADO" || Valor == "DEMORADO";

    public bool PermiteAbordaje =>
        Valor == "ABORDANDO";

    public bool PermiteNuevasReservas =>
        Valor == "PROGRAMADO" || Valor == "DEMORADO";

    public bool EstaFinalizado =>
        Valor == "ATERRIZADO" || Valor == "CANCELADO";

    public bool EstaEnCurso =>
        Valor == "ABORDANDO" || Valor == "EN_VUELO";

    // Transiciones válidas de estado — regla de negocio crítica
    public bool PuedeTransicionarA(EstadoFlight nuevoEstado)
    {
        return Valor switch
        {
            "PROGRAMADO" => nuevoEstado.Valor is
                "ABORDANDO" or "CANCELADO" or "DEMORADO",

            "DEMORADO" => nuevoEstado.Valor is
                "ABORDANDO" or "CANCELADO",

            "ABORDANDO" => nuevoEstado.Valor is
                "EN_VUELO" or "CANCELADO",

            "EN_VUELO" => nuevoEstado.Valor is
                "ATERRIZADO" or "DESVIADO",

            "DESVIADO" => nuevoEstado.Valor is
                "ATERRIZADO",

            "ATERRIZADO" => false,
            "CANCELADO"  => false,

            _ => false
        };
    }

    public override string ToString() => Valor;
}