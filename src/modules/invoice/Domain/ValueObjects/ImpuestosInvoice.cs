// src/modules/invoice/Domain/ValueObjects/ImpuestosInvoice.cs
namespace AirTicketSystem.modules.invoice.Domain.ValueObjects;

public sealed class ImpuestosInvoice
{
    public decimal Valor { get; }

    private ImpuestosInvoice(decimal valor) => Valor = valor;

    public static ImpuestosInvoice Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "Los impuestos de la factura no pueden ser negativos.");

        if (valor > 999_999_999)
            throw new ArgumentException(
                "Los impuestos no pueden superar 999.999.999.");

        return new ImpuestosInvoice(Math.Round(valor, 2));
    }

    public static ImpuestosInvoice Cero() => new(0);

    /// <summary>
    /// Calcula los impuestos aplicando un porcentaje sobre el subtotal.
    /// Ej: IVA del 19% en Colombia.
    /// </summary>
    public static ImpuestosInvoice CalcularDesde(decimal subtotal, decimal porcentaje)
    {
        if (subtotal < 0)
            throw new ArgumentException("El subtotal no puede ser negativo.");

        if (porcentaje < 0 || porcentaje > 100)
            throw new ArgumentException(
                "El porcentaje de impuesto debe estar entre 0 y 100.");

        return new ImpuestosInvoice(Math.Round(subtotal * (porcentaje / 100), 2));
    }

    public override string ToString() => $"{Valor:N2}";
}