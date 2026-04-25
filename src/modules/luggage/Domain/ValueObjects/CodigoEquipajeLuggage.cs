// src/modules/luggage/Domain/ValueObjects/CodigoEquipajeLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class CodigoEquipajeLuggage
{
    public string Valor { get; }

    private CodigoEquipajeLuggage(string valor) => Valor = valor;

    public static CodigoEquipajeLuggage Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código de equipaje no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!normalizado.StartsWith("EQ"))
            throw new ArgumentException(
                $"El código de equipaje debe comenzar con 'EQ'. Se recibió: '{valor}'");

        var digitosParte = normalizado[2..];

        if (digitosParte.Length != 8)
            throw new ArgumentException(
                $"El código de equipaje debe tener el formato EQ + 8 dígitos. Se recibió: '{valor}'");

        if (!digitosParte.All(char.IsDigit))
            throw new ArgumentException(
                $"Los caracteres después de 'EQ' deben ser dígitos. Se recibió: '{valor}'");

        return new CodigoEquipajeLuggage(normalizado);
    }

    /// <summary>
    /// Genera un código único de equipaje.
    /// Formato: EQ + 8 dígitos.
    /// Se combina timestamp con componente aleatorio para evitar
    /// colisiones en registros simultáneos durante check-in masivo.
    /// </summary>
    public static CodigoEquipajeLuggage Generar()
    {
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        var ultimosCuatro = timestamp[^4..];
        var aleatorio = new Random().Next(1000, 9999).ToString();
        var digitos = $"{ultimosCuatro}{aleatorio}";
        return new CodigoEquipajeLuggage($"EQ{digitos}");
    }

    public override string ToString() => Valor;
}