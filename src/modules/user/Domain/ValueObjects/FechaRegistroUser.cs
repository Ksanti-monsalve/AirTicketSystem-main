// src/modules/user/Domain/ValueObjects/FechaRegistroUser.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class FechaRegistroUser
{
    public DateTime Valor { get; }

    private FechaRegistroUser(DateTime valor) => Valor = valor;

    public static FechaRegistroUser Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de registro no puede ser una fecha vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException("La fecha de registro no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException("La fecha de registro no puede ser anterior al año 2000.");

        return new FechaRegistroUser(valor);
    }

    public static FechaRegistroUser Ahora() => new(DateTime.UtcNow);

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}