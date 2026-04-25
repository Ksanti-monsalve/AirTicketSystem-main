// src/modules/checkin/Domain/ValueObjects/FechaCheckinCheckin.cs
namespace AirTicketSystem.modules.checkin.Domain.ValueObjects;

public sealed class FechaCheckinCheckin
{
    public DateTime Valor { get; }

    private FechaCheckinCheckin(DateTime valor) => Valor = valor;

    public static FechaCheckinCheckin Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha del check-in no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha del check-in no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha del check-in no puede ser anterior al año 2000.");

        return new FechaCheckinCheckin(valor);
    }

    public static FechaCheckinCheckin Ahora() => new(DateTime.UtcNow);

    /// <summary>
    /// Valida que el check-in se realizó dentro de la ventana
    /// permitida respecto a la fecha de salida del vuelo.
    /// </summary>
    public bool EstaEnVentanaValida(
        DateTime apertura,
        DateTime cierre)
    {
        return Valor >= apertura && Valor <= cierre;
    }

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}