// src/modules/airline/Domain/ValueObjects/EsPrincipalAirlinePhone.cs
namespace AirTicketSystem.modules.airline.Domain.ValueObjects;

public sealed class EsPrincipalAirlinePhone
{
    public bool Valor { get; }

    private EsPrincipalAirlinePhone(bool valor) => Valor = valor;

    public static EsPrincipalAirlinePhone Crear(bool valor) => new(valor);

    public static EsPrincipalAirlinePhone Principal() => new(true);

    public static EsPrincipalAirlinePhone Secundario() => new(false);

    public override string ToString() => Valor ? "Principal" : "Secundario";
}