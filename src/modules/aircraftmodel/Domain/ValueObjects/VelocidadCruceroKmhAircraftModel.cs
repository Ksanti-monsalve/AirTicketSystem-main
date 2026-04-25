// src/modules/aircraftmodel/Domain/ValueObjects/VelocidadCruceroKmhAircraftModel.cs
namespace AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

public sealed class VelocidadCruceroKmhAircraftModel
{
    public int Valor { get; }

    private VelocidadCruceroKmhAircraftModel(int valor) => Valor = valor;

    public static VelocidadCruceroKmhAircraftModel Crear(int valor)
    {
        // Velocidad mínima de crucero comercial: ~600 km/h
        if (valor < 400)
            throw new ArgumentException(
                "La velocidad de crucero debe ser al menos 400 km/h.");

        // Velocidad máxima de crucero comercial: ~1.000 km/h
        if (valor > 1_200)
            throw new ArgumentException(
                "La velocidad de crucero no puede superar 1.200 km/h.");

        return new VelocidadCruceroKmhAircraftModel(valor);
    }

    public double EnNudos => Math.Round(Valor * 0.539957, 2);

    public override string ToString() => $"{Valor} km/h";
}