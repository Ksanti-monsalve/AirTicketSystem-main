// src/modules/specialty/Domain/ValueObjects/NombreSpecialty.cs
namespace AirTicketSystem.modules.specialty.Domain.ValueObjects;

public sealed class NombreSpecialty
{
    public string Valor { get; }

    private NombreSpecialty(string valor) => Valor = valor;

    public static NombreSpecialty Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre de la especialidad no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre de la especialidad debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre de la especialidad no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre de la especialidad no puede contener números.");

        return new NombreSpecialty(normalizado);
    }

    public override string ToString() => Valor;
}