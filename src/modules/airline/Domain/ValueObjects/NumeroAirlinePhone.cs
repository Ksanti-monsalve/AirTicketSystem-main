// src/modules/airline/Domain/ValueObjects/NumeroAirlinePhone.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class NumeroAirlinePhone
{
    public string Valor { get; }

    private NumeroAirlinePhone(string valor) => Valor = valor;

    public static NumeroAirlinePhone Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El número de teléfono de la aerolínea no puede estar vacío.");

        var limpio = new string(valor
            .Where(c => char.IsDigit(c) || c == '-' || c == ' ')
            .ToArray()).Trim();

        if (limpio.Length < 7)
            throw new ArgumentException("El teléfono debe tener al menos 7 dígitos.");

        if (limpio.Length > 20)
            throw new ArgumentException("El teléfono no puede superar 20 caracteres.");

        var soloDigitos = limpio.Where(char.IsDigit).ToArray();

        if (soloDigitos.Length < 7)
            throw new ArgumentException("El teléfono debe contener al menos 7 dígitos numéricos.");

        return new NumeroAirlinePhone(limpio);
    }

    public override string ToString() => Valor;
}