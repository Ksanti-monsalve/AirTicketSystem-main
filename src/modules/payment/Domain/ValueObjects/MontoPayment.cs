// src/modules/payment/Domain/ValueObjects/MontoPayment.cs
namespace AirTicketSystem.modules.payment.Domain.ValueObjects;

public sealed class MontoPayment
{
    public decimal Valor { get; }

    private MontoPayment(decimal valor) => Valor = valor;

    public static MontoPayment Crear(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "El monto del pago debe ser mayor a 0.");

        if (valor > 999_999_999)
            throw new ArgumentException(
                "El monto del pago no puede superar 999.999.999.");

        return new MontoPayment(Math.Round(valor, 2));
    }

    /// <summary>
    /// Verifica si este monto cubre el valor total requerido.
    /// Útil para validar pagos parciales o pagos exactos.
    /// </summary>
    public bool CubreValor(decimal valorRequerido)
        => Valor >= valorRequerido;

    /// <summary>
    /// Calcula el cambio cuando el monto supera el valor requerido.
    /// Retorna 0 si el monto no es suficiente.
    /// </summary>
    public decimal CambioContra(decimal valorRequerido)
    {
        var cambio = Valor - valorRequerido;
        return cambio > 0 ? Math.Round(cambio, 2) : 0;
    }

    public override string ToString() => $"{Valor:N2}";
}