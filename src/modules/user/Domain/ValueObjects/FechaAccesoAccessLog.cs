// src/modules/user/Domain/ValueObjects/FechaAccesoAccessLog.cs
namespace AirTicketSystem.modules.user.Domain.ValueObjects;

public sealed class FechaAccesoAccessLog
{
    public DateTime Valor { get; }

    private FechaAccesoAccessLog(DateTime valor) => Valor = valor;

    public static FechaAccesoAccessLog Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de acceso no puede ser una fecha vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException("La fecha de acceso no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException("La fecha de acceso no puede ser anterior al año 2000.");

        return new FechaAccesoAccessLog(valor);
    }

    public static FechaAccesoAccessLog Ahora() => new(DateTime.UtcNow);

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}