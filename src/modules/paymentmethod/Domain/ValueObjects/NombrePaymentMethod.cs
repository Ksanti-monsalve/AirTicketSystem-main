// src/modules/paymentmethod/Domain/ValueObjects/NombrePaymentMethod.cs
namespace AirTicketSystem.modules.paymentmethod.Domain.ValueObjects;

public sealed class NombrePaymentMethod
{
    public string Valor { get; }

    private NombrePaymentMethod(string valor) => Valor = valor;

    public static NombrePaymentMethod Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del método de pago no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException(
                "El nombre del método de pago debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException(
                "El nombre del método de pago no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException(
                "El nombre del método de pago no puede contener números.");

        return new NombrePaymentMethod(normalizado);
    }

    public override string ToString() => Valor;
}