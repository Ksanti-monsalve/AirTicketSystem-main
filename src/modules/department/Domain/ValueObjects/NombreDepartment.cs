// src/modules/department/Domain/ValueObjects/NombreDepartment.cs
namespace AirTicketSystem.modules.department.Domain.ValueObjects;

public sealed class NombreDepartment
{
    public string Valor { get; }

    private NombreDepartment(string valor) => Valor = valor;

    public static NombreDepartment Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del departamento/estado no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del departamento debe tener al menos 2 caracteres.");

        if (normalizado.Length > 100)
            throw new ArgumentException("El nombre del departamento no puede superar 100 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre del departamento no puede contener números.");

        return new NombreDepartment(normalizado);
    }

    public override string ToString() => Valor;
}