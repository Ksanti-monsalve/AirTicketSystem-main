// src/modules/flightcrew/Domain/ValueObjects/RolEnVueloFlightCrew.cs
namespace AirTicketSystem.modules.flightcrew.Domain.ValueObjects;

public sealed class RolEnVueloFlightCrew
{
    private static readonly string[] _rolesValidos =
    {
        "PILOTO",
        "COPILOTO",
        "SOBRECARGO",
        "AUXILIAR_VUELO",
        "AUXILIAR_SEGURIDAD"
    };

    public string Valor { get; }

    private RolEnVueloFlightCrew(string valor) => Valor = valor;

    public static RolEnVueloFlightCrew Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El rol en el vuelo no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_rolesValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El rol '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _rolesValidos)}");

        return new RolEnVueloFlightCrew(normalizado);
    }

    // Métodos de fábrica expresivos
    public static RolEnVueloFlightCrew Piloto() => new("PILOTO");
    public static RolEnVueloFlightCrew Copiloto() => new("COPILOTO");
    public static RolEnVueloFlightCrew Sobrecargo() => new("SOBRECARGO");
    public static RolEnVueloFlightCrew AuxiliarVuelo() => new("AUXILIAR_VUELO");
    public static RolEnVueloFlightCrew AuxiliarSeguridad() => new("AUXILIAR_SEGURIDAD");

    // Propiedades de negocio
    public bool EsParteDeCabina =>
        Valor == "PILOTO" || Valor == "COPILOTO";

    public bool EsParteDeCabina_Pasajeros =>
        Valor == "SOBRECARGO" ||
        Valor == "AUXILIAR_VUELO" ||
        Valor == "AUXILIAR_SEGURIDAD";

    public bool EsObligatorioParaDespegar =>
        Valor == "PILOTO" || Valor == "COPILOTO";

    public override string ToString() => Valor;
}