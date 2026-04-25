// src/modules/aircraft/Domain/ValueObjects/EstadoAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class EstadoAircraft
{
    private static readonly string[] _estadosValidos =
    {
        "DISPONIBLE",
        "EN_VUELO",
        "MANTENIMIENTO",
        "FUERA_DE_SERVICIO"
    };

    public string Valor { get; }

    private EstadoAircraft(string valor) => Valor = valor;

    public static EstadoAircraft Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del avión no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoAircraft(normalizado);
    }

    public static EstadoAircraft Disponible() => new("DISPONIBLE");
    public static EstadoAircraft EnVuelo() => new("EN_VUELO");
    public static EstadoAircraft EnMantenimiento() => new("MANTENIMIENTO");
    public static EstadoAircraft FueraDeServicio() => new("FUERA_DE_SERVICIO");

    public bool EstaOperativo =>
        Valor == "DISPONIBLE" || Valor == "EN_VUELO";

    public bool PuedeAsignarse => Valor == "DISPONIBLE";

    public override string ToString() => Valor;
}