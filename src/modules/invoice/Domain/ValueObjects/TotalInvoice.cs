// src/modules/invoice/Domain/ValueObjects/TotalInvoice.cs
namespace AirTicketSystem.modules.invoice.Domain.ValueObjects;

public sealed class TotalInvoice
{
    public decimal Valor { get; }

    private TotalInvoice(decimal valor) => Valor = valor;

    public static TotalInvoice Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El total de la factura no puede ser negativo.");

        if (valor == 0)
            throw new ArgumentException(
                "El total de la factura debe ser mayor a 0.");

        if (valor > 999_999_999)
            throw new ArgumentException(
                "El total de la factura no puede superar 999.999.999.");

        return new TotalInvoice(Math.Round(valor, 2));
    }

    /// <summary>
    /// Calcula y valida que el total sea la suma exacta
    /// de subtotal más impuestos. Garantiza consistencia
    /// en la factura antes de persistirla.
    /// </summary>
    public static TotalInvoice Calcular(decimal subtotal, decimal impuestos)
    {
        if (subtotal < 0)
            throw new ArgumentException("El subtotal no puede ser negativo.");

        if (impuestos < 0)
            throw new ArgumentException("Los impuestos no pueden ser negativos.");

        if (subtotal == 0)
            throw new ArgumentException("El subtotal debe ser mayor a 0.");

        return new TotalInvoice(Math.Round(subtotal + impuestos, 2));
    }

    /// <summary>
    /// Verifica que el total sea coherente con subtotal + impuestos.
    /// Útil para validar facturas recibidas de sistemas externos.
    /// </summary>
    public bool EsCoherenteCon(decimal subtotal, decimal impuestos)
    {
        var esperado = Math.Round(subtotal + impuestos, 2);
        return Valor == esperado;
    }

    public override string ToString() => $"{Valor:N2}";
}