// src/modules/boardingpass/Domain/ValueObjects/CodigoPaseBoardingPass.cs
namespace AirTicketSystem.modules.boardingpass.Domain.ValueObjects;

public sealed class CodigoPaseBoardingPass
{
    public string Valor { get; }

    private CodigoPaseBoardingPass(string valor) => Valor = valor;

    public static CodigoPaseBoardingPass Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código del pase de abordar no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!normalizado.StartsWith("BP"))
            throw new ArgumentException(
                $"El código del pase debe comenzar con 'BP'. Se recibió: '{valor}'");

        var restante = normalizado[2..];

        if (restante.Length != 10)
            throw new ArgumentException(
                $"El código del pase debe tener el formato BP + 10 caracteres. Se recibió: '{valor}'");

        if (!restante.All(char.IsLetterOrDigit))
            throw new ArgumentException(
                $"Los caracteres después de 'BP' solo pueden ser letras y números. Se recibió: '{valor}'");

        return new CodigoPaseBoardingPass(normalizado);
    }

    /// <summary>
    /// Genera un código único de pase de abordar.
    /// Formato: BP + 10 caracteres alfanuméricos.
    /// Combina timestamp con componente aleatorio para unicidad.
    /// </summary>
    public static CodigoPaseBoardingPass Generar()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        var ultimosSeis = timestamp[^6..];
        var random = new Random();
        var aleatorio = new string(Enumerable.Repeat(chars, 4)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return new CodigoPaseBoardingPass($"BP{ultimosSeis}{aleatorio}");
    }

    public override string ToString() => Valor;
}