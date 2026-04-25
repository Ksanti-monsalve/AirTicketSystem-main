// src/modules/aircraftmodel/Domain/ValueObjects/AutonomiKmAircraftModel.cs
namespace AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

public sealed class AutonomiKmAircraftModel
{
    public int Valor { get; }

    private AutonomiKmAircraftModel(int valor) => Valor = valor;

    public static AutonomiKmAircraftModel Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException("La autonomía del avión debe ser mayor a 0 km.");

        // Avión comercial con mayor autonomía: Boeing 777X ~13.500 km
        if (valor > 20_000)
            throw new ArgumentException(
                "La autonomía no puede superar 20.000 km.");

        return new AutonomiKmAircraftModel(valor);
    }

    public double EnMillas => Math.Round(Valor * 0.621371, 2);

    public override string ToString() => $"{Valor} km";
}