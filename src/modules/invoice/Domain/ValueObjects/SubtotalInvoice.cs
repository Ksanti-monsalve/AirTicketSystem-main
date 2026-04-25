// src/modules/invoice/Domain/ValueObjects/SubtotalInvoice.cs
namespace AirTicketSystem.modules.invoice.Domain.ValueObjects;

public sealed class SubtotalInvoice
{
    public decimal Valor { get; }

    private SubtotalInvoice(decimal valor) => Valor = valor;

    public static SubtotalInvoice Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El subtotal de la factura no puede ser negativo.");

        if (valor == 0)
            throw new ArgumentException(
                "El subtotal de la factura debe ser mayor a 0.");

        if (valor > 999_999_999)
            throw new ArgumentException(
                "El subtotal no puede superar 999.999.999.");

        return new SubtotalInvoice(Math.Round(valor, 2));
    }

    public override string ToString() => $"{Valor:N2}";
}