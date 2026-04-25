// src/modules/user/Domain/ValueObjects/ActivoUser.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class ActivoUser
{
    public bool Valor { get; }

    private ActivoUser(bool valor) => Valor = valor;

    public static ActivoUser Crear(bool valor) => new(valor);

    public static ActivoUser Activo() => new(true);

    public static ActivoUser Inactivo() => new(false);

    public override string ToString() => Valor ? "Activo" : "Inactivo";
}