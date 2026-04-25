// src/modules/boardingpass/Domain/ValueObjects/FechaEmisionBoardingPass.cs
namespace AirTicketSystem.modules.boardingpass.Domain.ValueObjects;

public sealed class FechaEmisionBoardingPass
{
    public DateTime Valor { get; }

    private FechaEmisionBoardingPass(DateTime valor) => Valor = valor;

    public static FechaEmisionBoardingPass Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de emisión del pase no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de emisión del pase no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de emisión del pase no puede ser anterior al año 2000.");

        return new FechaEmisionBoardingPass(valor);
    }

    public static FechaEmisionBoardingPass Ahora() => new(DateTime.UtcNow);

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}