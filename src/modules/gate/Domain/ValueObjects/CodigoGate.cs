// src/modules/gate/Domain/ValueObjects/CodigoGate.cs
namespace AirTicketSystem.modules.gate.Domain.ValueObjects;

public sealed class CodigoGate
{
    public string Valor { get; }

    private CodigoGate(string valor) => Valor = valor;

    public static CodigoGate Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código de la puerta de embarque no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 1)
            throw new ArgumentException("El código de la puerta debe tener al menos 1 carácter.");

        if (normalizado.Length > 10)
            throw new ArgumentException("El código de la puerta no puede superar 10 caracteres.");

        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"El código de la puerta solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new CodigoGate(normalizado);
    }

    public override string ToString() => Valor;
}