// src/modules/checkin/Domain/ValueObjects/EstadoCheckin.cs
namespace AirTicketSystem.modules.checkin.Domain.ValueObjects;

public sealed class EstadoCheckin
{
    private static readonly string[] _estadosValidos =
    {
        "PENDIENTE",
        "COMPLETADO",
        "CANCELADO"
    };

    public string Valor { get; }

    private EstadoCheckin(string valor) => Valor = valor;

    public static EstadoCheckin Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del check-in no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoCheckin(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoCheckin Pendiente() => new("PENDIENTE");
    public static EstadoCheckin Completado() => new("COMPLETADO");
    public static EstadoCheckin Cancelado() => new("CANCELADO");

    // Propiedades de negocio
    public bool EstaCompletado => Valor == "COMPLETADO";
    public bool EstaPendiente => Valor == "PENDIENTE";
    public bool EstaCancelado => Valor == "CANCELADO";

    /// <summary>
    /// Solo un check-in completado permite generar el pase de abordar.
    /// </summary>
    public bool PermiteGenerarPaseAbordar => Valor == "COMPLETADO";

    /// <summary>
    /// Un check-in pendiente puede completarse o cancelarse.
    /// Uno completado no puede revertirse — hay que anular el tiquete.
    /// </summary>
    public bool PuedeTransicionarA(EstadoCheckin nuevoEstado)
    {
        return Valor switch
        {
            "PENDIENTE"  => nuevoEstado.Valor is "COMPLETADO" or "CANCELADO",
            "COMPLETADO" => false,
            "CANCELADO"  => false,
            _            => false
        };
    }

    public override string ToString() => Valor;
}