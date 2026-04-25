// src/modules/aircraftseat/Domain/ValueObjects/FilaAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class FilaAircraftSeat
{
    public int Valor { get; }

    private FilaAircraftSeat(int valor) => Valor = valor;

    public static FilaAircraftSeat Crear(int valor)
    {
        if (valor < 1)
            throw new ArgumentException("El número de fila debe ser al menos 1.");

        if (valor > 99)
            throw new ArgumentException("El número de fila no puede superar 99.");

        return new FilaAircraftSeat(valor);
    }

    public override string ToString() => Valor.ToString();
}