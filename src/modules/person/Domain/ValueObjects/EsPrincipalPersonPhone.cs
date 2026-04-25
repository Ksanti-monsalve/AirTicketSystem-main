// src/modules/person/Domain/ValueObjects/EsPrincipalPersonPhone.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class EsPrincipalPersonPhone
{
    public bool Valor { get; }

    private EsPrincipalPersonPhone(bool valor) => Valor = valor;

    public static EsPrincipalPersonPhone Crear(bool valor) => new(valor);

    public static EsPrincipalPersonPhone Principal() => new(true);

    public static EsPrincipalPersonPhone Secundario() => new(false);

    public override string ToString() => Valor ? "Principal" : "Secundario";
}