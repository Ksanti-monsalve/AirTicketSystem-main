// src/modules/department/Domain/ValueObjects/CodigoDepartment.cs
namespace AirTicketSystem.modules.department.Domain.ValueObjects;

public sealed class CodigoDepartment
{
    public string Valor { get; }

    private CodigoDepartment(string valor) => Valor = valor;

    public static CodigoDepartment Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código del departamento no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 2)
            throw new ArgumentException("El código del departamento debe tener al menos 2 caracteres.");

        if (normalizado.Length > 10)
            throw new ArgumentException("El código del departamento no puede superar 10 caracteres.");

        if (!normalizado.All(char.IsLetterOrDigit))
            throw new ArgumentException(
                $"El código del departamento solo puede contener letras y números. Se recibió: '{valor}'");

        return new CodigoDepartment(normalizado);
    }

    public override string ToString() => Valor;
}