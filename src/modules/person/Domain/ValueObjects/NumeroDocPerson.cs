// src/modules/person/Domain/ValueObjects/NumeroDocPerson.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class NumeroDocPerson
{
    public string Valor { get; }

    private NumeroDocPerson(string valor) => Valor = valor;

    public static NumeroDocPerson Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El número de documento no puede estar vacío.");

        var normalizado = valor.Trim().Replace(" ", "").Replace("-", "").ToUpperInvariant();

        if (normalizado.Length < 5)
            throw new ArgumentException("El número de documento debe tener al menos 5 caracteres.");

        if (normalizado.Length > 20)
            throw new ArgumentException("El número de documento no puede superar 20 caracteres.");

        if (!normalizado.All(char.IsLetterOrDigit))
            throw new ArgumentException(
                $"El número de documento solo puede contener letras y números. Se recibió: '{valor}'");

        return new NumeroDocPerson(normalizado);
    }

    public override string ToString() => Valor;
}