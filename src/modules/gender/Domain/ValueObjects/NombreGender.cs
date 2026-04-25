// src/modules/gender/Domain/ValueObjects/NombreGender.cs
namespace AirTicketSystem.modules.gender.Domain.ValueObjects;

public sealed class NombreGender
{
    public string Valor { get; }

    private NombreGender(string valor) => Valor = valor;

    public static NombreGender Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del género no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del género debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El nombre del género no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre del género no puede contener números.");

        return new NombreGender(normalizado);
    }

    public override string ToString() => Valor;
}