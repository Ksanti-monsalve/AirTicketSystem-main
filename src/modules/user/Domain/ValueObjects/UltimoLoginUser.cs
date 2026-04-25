// src/modules/user/Domain/ValueObjects/UltimoLoginUser.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class UltimoLoginUser
{
    public DateTime Valor { get; }

    private UltimoLoginUser(DateTime valor) => Valor = valor;

    public static UltimoLoginUser Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de último login no puede ser una fecha vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException("La fecha de último login no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException("La fecha de último login no puede ser anterior al año 2000.");

        return new UltimoLoginUser(valor);
    }

    public static UltimoLoginUser Ahora() => new(DateTime.UtcNow);

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}