// src/modules/payment/Domain/ValueObjects/EstadoPayment.cs
namespace AirTicketSystem.modules.payment.Domain.ValueObjects;

public sealed class EstadoPayment
{
    private static readonly string[] _estadosValidos =
    {
        "PENDIENTE",
        "APROBADO",
        "RECHAZADO",
        "REEMBOLSADO"
    };

    public string Valor { get; }

    private EstadoPayment(string valor) => Valor = valor;

    public static EstadoPayment Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del pago no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoPayment(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoPayment Pendiente() => new("PENDIENTE");
    public static EstadoPayment Aprobado() => new("APROBADO");
    public static EstadoPayment Rechazado() => new("RECHAZADO");
    public static EstadoPayment Reembolsado() => new("REEMBOLSADO");

    // Propiedades de negocio
    public bool EstaAprobado => Valor == "APROBADO";
    public bool EstaPendiente => Valor == "PENDIENTE";
    public bool EstaRechazado => Valor == "RECHAZADO";
    public bool EstaReembolsado => Valor == "REEMBOLSADO";

    /// <summary>
    /// Solo un pago aprobado confirma la reserva y permite
    /// emitir el tiquete.
    /// </summary>
    public bool ConfirmaReserva => Valor == "APROBADO";

    /// <summary>
    /// Un pago rechazado permite reintentar con otro método.
    /// </summary>
    public bool PermiteReintento => Valor == "RECHAZADO";

    /// <summary>
    /// Solo un pago aprobado puede reembolsarse.
    /// </summary>
    public bool PuedeReembolsarse => Valor == "APROBADO";

    // Máquina de estados — transiciones válidas
    public bool PuedeTransicionarA(EstadoPayment nuevoEstado)
    {
        return Valor switch
        {
            "PENDIENTE"   => nuevoEstado.Valor is "APROBADO" or "RECHAZADO",
            "APROBADO"    => nuevoEstado.Valor is "REEMBOLSADO",
            "RECHAZADO"   => nuevoEstado.Valor is "PENDIENTE",
            "REEMBOLSADO" => false,
            _             => false
        };
    }

    public override string ToString() => Valor;
}