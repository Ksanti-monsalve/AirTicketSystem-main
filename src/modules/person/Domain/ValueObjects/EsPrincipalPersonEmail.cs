// src/modules/person/Domain/ValueObjects/EsPrincipalPersonEmail.cs
namespace AirTicketSystem.modules.person.Domain.ValueObjects;

public sealed class EsPrincipalPersonEmail
{
    public bool Valor { get; }

    private EsPrincipalPersonEmail(bool valor) => Valor = valor;

    public static EsPrincipalPersonEmail Crear(bool valor) => new(valor);

    public static EsPrincipalPersonEmail Principal() => new(true);

    public static EsPrincipalPersonEmail Secundario() => new(false);

    public override string ToString() => Valor ? "Principal" : "Secundario";
}