// src/modules/luggage/Domain/ValueObjects/EstadoLuggage.cs
namespace AirTicketSystem.modules.luggage.Domain.ValueObjects;

public sealed class EstadoLuggage
{
    private static readonly string[] _estadosValidos =
    {
        "DECLARADO",
        "REGISTRADO",
        "EN_BODEGA",
        "EN_DESTINO",
        "ENTREGADO",
        "PERDIDO",
        "DAÑADO"
    };

    public string Valor { get; }

    private EstadoLuggage(string valor) => Valor = valor;

    public static EstadoLuggage Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado del equipaje no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoLuggage(normalizado);
    }

    // Métodos de fábrica expresivos
    public static EstadoLuggage Declarado() => new("DECLARADO");
    public static EstadoLuggage Registrado() => new("REGISTRADO");
    public static EstadoLuggage EnBodega() => new("EN_BODEGA");
    public static EstadoLuggage EnDestino() => new("EN_DESTINO");
    public static EstadoLuggage Entregado() => new("ENTREGADO");
    public static EstadoLuggage Perdido() => new("PERDIDO");
    public static EstadoLuggage Danado() => new("DAÑADO");

    // Propiedades de negocio
    public bool RequiereCodigoFisico =>
        Valor != "DECLARADO";

    public bool EstaEnTransito =>
        Valor == "REGISTRADO" ||
        Valor == "EN_BODEGA"  ||
        Valor == "EN_DESTINO";

    public bool EstaFinalizado =>
        Valor == "ENTREGADO" ||
        Valor == "PERDIDO"   ||
        Valor == "DAÑADO";

    public bool TieneIncidencia =>
        Valor == "PERDIDO" || Valor == "DAÑADO";

    // Máquina de estados — transiciones válidas
    public bool PuedeTransicionarA(EstadoLuggage nuevoEstado)
    {
        return Valor switch
        {
            "DECLARADO"  => nuevoEstado.Valor is "REGISTRADO",
            "REGISTRADO" => nuevoEstado.Valor is "EN_BODEGA",
            "EN_BODEGA"  => nuevoEstado.Valor is "EN_DESTINO" or "PERDIDO" or "DAÑADO",
            "EN_DESTINO" => nuevoEstado.Valor is "ENTREGADO"  or "PERDIDO" or "DAÑADO",
            "ENTREGADO"  => false,
            "PERDIDO"    => false,
            "DAÑADO"     => false,
            _            => false
        };
    }

    public override string ToString() => Valor;
}