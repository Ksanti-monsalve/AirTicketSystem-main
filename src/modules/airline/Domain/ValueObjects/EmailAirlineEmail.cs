// src/modules/airline/Domain/ValueObjects/EmailAirlineEmail.cs
using System.Text.RegularExpressions;

namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class EmailAirlineEmail
{
    private static readonly Regex _regex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Valor { get; }

    private EmailAirlineEmail(string valor) => Valor = valor;

    public static EmailAirlineEmail Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El email de la aerolínea no puede estar vacío.");

        var normalizado = valor.Trim().ToLowerInvariant();

        if (normalizado.Length > 150)
            throw new ArgumentException("El email no puede superar 150 caracteres.");

        if (!_regex.IsMatch(normalizado))
            throw new ArgumentException(
                $"'{valor}' no es un formato de email válido.");

        return new EmailAirlineEmail(normalizado);
    }

    public override string ToString() => Valor;
}