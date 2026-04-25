// src/modules/person/Domain/ValueObjects/EsPrincipalPersonAddress.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class EsPrincipalPersonAddress
{
    public bool Valor { get; }

    private EsPrincipalPersonAddress(bool valor) => Valor = valor;

    public static EsPrincipalPersonAddress Crear(bool valor) => new(valor);

    public static EsPrincipalPersonAddress Principal() => new(true);

    public static EsPrincipalPersonAddress Secundario() => new(false);

    public override string ToString() => Valor ? "Principal" : "Secundario";
}