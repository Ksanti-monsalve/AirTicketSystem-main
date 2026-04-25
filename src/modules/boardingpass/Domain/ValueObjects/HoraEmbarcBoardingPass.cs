// src/modules/boardingpass/Domain/ValueObjects/HoraEmbarcBoardingPass.cs
namespace AirTicketSystem.modules.boardingpass.Domain.ValueObjects;

public sealed class HoraEmbarcBoardingPass
{
    public DateTime Valor { get; }

    private HoraEmbarcBoardingPass(DateTime valor) => Valor = valor;

    public static HoraEmbarcBoardingPass Crear(DateTime valor, DateTime fechaSalida)
    {
        if (valor == default)
            throw new ArgumentException("La hora de embarque no puede estar vacía.");

        // El embarque abre entre 60 y 30 minutos antes del vuelo
        if (valor >= fechaSalida)
            throw new ArgumentException(
                "La hora de embarque debe ser anterior a la salida del vuelo.");

        if (valor < fechaSalida.AddMinutes(-60))
            throw new ArgumentException(
                "La hora de embarque no puede ser más de 60 minutos antes del vuelo.");

        return new HoraEmbarcBoardingPass(valor);
    }

    // Bypasses the 30-60 min window rule — only for reconstituting from DB-stored values.
    public static HoraEmbarcBoardingPass Reconstituir(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La hora de embarque no puede estar vacía.");
        return new HoraEmbarcBoardingPass(valor);
    }

    public bool YaComenzo => DateTime.UtcNow >= Valor;

    public int MinutosRestantes
    {
        get
        {
            var diferencia = Valor - DateTime.UtcNow;
            return diferencia.TotalMinutes > 0
                ? (int)diferencia.TotalMinutes
                : 0;
        }
    }

    public string EnFormatoCorto => Valor.ToString("HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm");
}