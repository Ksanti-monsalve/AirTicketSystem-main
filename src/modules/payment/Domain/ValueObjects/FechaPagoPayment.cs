// src/modules/payment/Domain/ValueObjects/FechaPagoPayment.cs
namespace AirTicketSystem.modules.payment.Domain.ValueObjects;

public sealed class FechaPagoPayment
{
    public DateTime Valor { get; }

    private FechaPagoPayment(DateTime valor) => Valor = valor;

    public static FechaPagoPayment Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de pago no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de pago no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de pago no puede ser anterior al año 2000.");

        return new FechaPagoPayment(valor);
    }

    public static FechaPagoPayment Ahora() => new(DateTime.UtcNow);

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}