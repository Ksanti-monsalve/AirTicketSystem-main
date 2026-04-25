// src/modules/person/Domain/ValueObjects/IndicativoPaisPersonPhone.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class IndicativoPaisPersonPhone
{
    public string Valor { get; }

    private IndicativoPaisPersonPhone(string valor) => Valor = valor;

    public static IndicativoPaisPersonPhone Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El indicativo de país no puede estar vacío.");

        var normalizado = valor.Trim();

        if (!normalizado.StartsWith('+'))
            throw new ArgumentException("El indicativo de país debe comenzar con '+'.");

        if (normalizado.Length < 2)
            throw new ArgumentException("El indicativo debe tener al menos 2 caracteres.");

        if (normalizado.Length > 5)
            throw new ArgumentException("El indicativo no puede superar 5 caracteres.");

        var soloDigitos = normalizado[1..];
        if (!soloDigitos.All(char.IsDigit))
            throw new ArgumentException(
                $"El indicativo solo puede contener el signo '+' seguido de dígitos. Se recibió: '{valor}'");

        return new IndicativoPaisPersonPhone(normalizado);
    }

    public override string ToString() => Valor;
}