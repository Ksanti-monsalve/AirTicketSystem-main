// src/modules/invoice/Domain/ValueObjects/NumeroFacturaInvoice.cs
namespace AirTicketSystem.modules.invoice.Domain.ValueObjects;

public sealed class NumeroFacturaInvoice
{
    public string Valor { get; }

    private NumeroFacturaInvoice(string valor) => Valor = valor;

    public static NumeroFacturaInvoice Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El número de factura no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!normalizado.StartsWith("FAC"))
            throw new ArgumentException(
                $"El número de factura debe comenzar con 'FAC'. Se recibió: '{valor}'");

        var restante = normalizado[3..];

        if (restante.Length < 4 || restante.Length > 27)
            throw new ArgumentException(
                $"El número de factura debe tener el formato FAC + 4 a 27 caracteres. Se recibió: '{valor}'");

        if (!restante.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"El número de factura solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new NumeroFacturaInvoice(normalizado);
    }

    /// <summary>
    /// Genera un número de factura único.
    /// Formato: FAC-[AÑO]-[timestamp]-[aleatorio]
    /// Ej: FAC-2025-834521-4829
    /// </summary>
    public static NumeroFacturaInvoice Generar()
    {
        var anio = DateTime.UtcNow.Year;
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        var ultimosSeis = timestamp[^6..];
        var aleatorio = new Random().Next(1000, 9999).ToString();
        return new NumeroFacturaInvoice($"FAC-{anio}-{ultimosSeis}-{aleatorio}");
    }

    public override string ToString() => Valor;
}