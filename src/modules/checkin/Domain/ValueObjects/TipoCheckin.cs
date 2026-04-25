// src/modules/checkin/Domain/ValueObjects/TipoCheckin.cs
namespace AirTicketSystem.modules.checkin.Domain.ValueObjects;

public sealed class TipoCheckin
{
    private static readonly string[] _tiposValidos =
    {
        "VIRTUAL",
        "PRESENCIAL"
    };

    public string Valor { get; }

    private TipoCheckin(string valor) => Valor = valor;

    public static TipoCheckin Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El tipo de check-in no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_tiposValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El tipo de check-in '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _tiposValidos)}");

        return new TipoCheckin(normalizado);
    }

    // Métodos de fábrica expresivos
    public static TipoCheckin Virtual() => new("VIRTUAL");
    public static TipoCheckin Presencial() => new("PRESENCIAL");

    // Propiedades de negocio
    public bool EsVirtual => Valor == "VIRTUAL";
    public bool EsPresencial => Valor == "PRESENCIAL";

    /// <summary>
    /// El check-in virtual no requiere trabajador asignado.
    /// El presencial sí — se valida en el caso de uso.
    /// </summary>
    public bool RequiereTrabajador => Valor == "PRESENCIAL";

    /// <summary>
    /// El check-in virtual puede hacerse hasta 1 hora antes del vuelo.
    /// El presencial hasta 45 minutos antes.
    /// </summary>
    public int MinutosLimiteAntesDeVuelo => Valor == "VIRTUAL" ? 60 : 45;

    public override string ToString() => Valor;
}