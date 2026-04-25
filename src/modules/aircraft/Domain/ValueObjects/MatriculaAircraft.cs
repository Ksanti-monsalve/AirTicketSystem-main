// src/modules/aircraft/Domain/ValueObjects/MatriculaAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class MatriculaAircraft
{
    public string Valor { get; }

    private MatriculaAircraft(string valor) => Valor = valor;

    public static MatriculaAircraft Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("La matrícula del avión no puede estar vacía.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (normalizado.Length < 4)
            throw new ArgumentException("La matrícula debe tener al menos 4 caracteres.");

        if (normalizado.Length > 20)
            throw new ArgumentException("La matrícula no puede superar 20 caracteres.");

        if (!normalizado.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException(
                $"La matrícula solo puede contener letras, números y guión. Se recibió: '{valor}'");

        return new MatriculaAircraft(normalizado);
    }

    public override string ToString() => Valor;
}