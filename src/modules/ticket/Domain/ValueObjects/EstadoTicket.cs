// src/modules/ticket/Domain/ValueObjects/EstadoTicket.cs
namespace AirTicketSystem.modules.ticket.Domain.ValueObjects;

public sealed class EstadoTicket
{
    private static readonly string[] _estadosValidos =
    {
        "EMITIDO",
        "CHECKIN_HECHO",
        "ABORDADO",
        "USADO",
        "ANULADO"
    };

    public string Valor { get; }

    private EstadoTicket(string valor) => Valor = valor;

    public static EstadoTicket Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del tiquete no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoTicket(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoTicket Emitido() => new("EMITIDO");
    public static EstadoTicket CheckinHecho() => new("CHECKIN_HECHO");
    public static EstadoTicket Abordado() => new("ABORDADO");
    public static EstadoTicket Usado() => new("USADO");
    public static EstadoTicket Anulado() => new("ANULADO");

    // Propiedades de negocio
    public bool PermiteCheckin =>
        Valor == "EMITIDO";

    public bool PermiteAbordaje =>
        Valor == "CHECKIN_HECHO";

    public bool EstaVigente =>
        Valor == "EMITIDO" ||
        Valor == "CHECKIN_HECHO" ||
        Valor == "ABORDADO";

    public bool EstaFinalizado =>
        Valor == "USADO" || Valor == "ANULADO";

    public bool PuedeAnularse =>
        Valor == "EMITIDO" || Valor == "CHECKIN_HECHO";

    // Máquina de estados — transiciones válidas
    public bool PuedeTransicionarA(EstadoTicket nuevoEstado)
    {
        return Valor switch
        {
            "EMITIDO"       => nuevoEstado.Valor is "CHECKIN_HECHO" or "ANULADO",
            "CHECKIN_HECHO" => nuevoEstado.Valor is "ABORDADO" or "ANULADO",
            "ABORDADO"      => nuevoEstado.Valor is "USADO",
            "USADO"         => false,
            "ANULADO"       => false,
            _               => false
        };
    }

    public override string ToString() => Valor;
}