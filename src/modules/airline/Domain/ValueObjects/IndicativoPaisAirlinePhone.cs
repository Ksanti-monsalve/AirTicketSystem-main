// src/modules/airline/Domain/ValueObjects/IndicativoPaisAirlinePhone.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class IndicativoPaisAirlinePhone
{
    public string Valor { get; }

    private IndicativoPaisAirlinePhone(string valor) => Valor = valor;

    public static IndicativoPaisAirlinePhone Crear(string? valor)
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

        return new IndicativoPaisAirlinePhone(normalizado);
    }

    public override string ToString() => Valor;
}