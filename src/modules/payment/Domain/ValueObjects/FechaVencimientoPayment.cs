// src/modules/payment/Domain/ValueObjects/FechaVencimientoPayment.cs
namespace AirTicketSystem.modules.payment.Domain.ValueObjects;

public sealed class FechaVencimientoPayment
{
    public DateTime Valor { get; }

    private FechaVencimientoPayment(DateTime valor) => Valor = valor;

    public static FechaVencimientoPayment Crear(DateTime valor, DateTime fechaCreacion)
    {
        if (valor == default)
            throw new ArgumentException(
                "La fecha de vencimiento del pago no puede estar vacía.");

        if (valor <= fechaCreacion)
            throw new ArgumentException(
                "La fecha de vencimiento debe ser posterior a la fecha de creación del pago.");

        // Un pago pendiente no puede tener más de 72 horas para completarse
        if (valor > fechaCreacion.AddHours(72))
            throw new ArgumentException(
                "La fecha de vencimiento no puede superar 72 horas desde la creación del pago.");

        return new FechaVencimientoPayment(valor);
    }

    /// <summary>
    /// Vencimiento estándar: 24 horas desde la creación del pago.
    /// </summary>
    public static FechaVencimientoPayment EstandarDesde(DateTime fechaCreacion)
        => new(fechaCreacion.AddHours(24));

    public static FechaVencimientoPayment Reconstituir(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException(
                "La fecha de vencimiento del pago no puede estar vacía.");
        return new FechaVencimientoPayment(valor);
    }

    public bool EstaVencido => DateTime.UtcNow > Valor;

    public int HorasRestantes
    {
        get
        {
            var diferencia = Valor - DateTime.UtcNow;
            return diferencia.TotalHours > 0
                ? (int)diferencia.TotalHours
                : 0;
        }
    }

    public bool VenceProximamente
    {
        get
        {
            var diferencia = Valor - DateTime.UtcNow;
            return diferencia.TotalHours > 0 && diferencia.TotalHours <= 2;
        }
    }

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}