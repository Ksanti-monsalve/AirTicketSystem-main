// src/modules/payment/Domain/ValueObjects/ReferenciaPayment.cs
namespace AirTicketSystem.modules.payment.Domain.ValueObjects;

public sealed class ReferenciaPayment
{
    public string Valor { get; }

    private ReferenciaPayment(string valor) => Valor = valor;

    public static ReferenciaPayment Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La referencia de pago no puede estar vacía.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 5)
            throw new ArgumentException(
                "La referencia de pago debe tener al menos 5 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException(
                "La referencia de pago no puede superar 100 caracteres.");

        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"La referencia de pago solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new ReferenciaPayment(normalizado);
    }

    /// <summary>
    /// Genera una referencia simulada de pago.
    /// Formato: REF + timestamp + componente aleatorio.
    /// En producción esta referencia la provee la pasarela de pago.
    /// </summary>
    public static ReferenciaPayment Generar()
    {
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        var ultimosSeis = timestamp[^6..];
        var aleatorio = new Random().Next(1000, 9999).ToString();
        return new ReferenciaPayment($"REF-{ultimosSeis}-{aleatorio}");
    }

    public override string ToString() => Valor;
}