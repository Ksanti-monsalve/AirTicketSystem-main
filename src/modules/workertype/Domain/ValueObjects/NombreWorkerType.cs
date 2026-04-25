// src/modules/workertype/Domain/ValueObjects/NombreWorkerType.cs
namespace AirTicketSystem.modules.workertype.Domain.ValueObjects;

public sealed class NombreWorkerType
{
    public string Valor { get; }

    private NombreWorkerType(string valor) => Valor = valor;

    public static NombreWorkerType Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El nombre del tipo de trabajador no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 2)
            throw new ArgumentException("El nombre del tipo de trabajador debe tener al menos 2 caracteres.");

        if (normalizado.Length > 50)
            throw new ArgumentException("El nombre del tipo de trabajador no puede superar 50 caracteres.");

        if (normalizado.Any(char.IsDigit))
            throw new ArgumentException("El nombre del tipo de trabajador no puede contener números.");

        return new NombreWorkerType(normalizado);
    }

    public override string ToString() => Valor;
}