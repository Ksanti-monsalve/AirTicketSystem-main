// src/UI/Admin/Aeronautica/AeronauticaMenu.cs
using AirTicketSystem.shared.UI;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AeronauticaMenu
{
    private readonly IServiceProvider _provider;

    public AeronauticaMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Gestión Aeronáutica");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "Seleccione un módulo",
                [
                    "Fabricantes de aeronaves",
                    "Modelos de aeronave",
                    "Aeronaves (flota)",
                    "Asientos de aeronave",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Fabricantes de aeronaves":
                    await new AircraftManufacturerMenu(_provider).MostrarAsync();
                    break;

                case "Modelos de aeronave":
                    await new AircraftModelMenu(_provider).MostrarAsync();
                    break;

                case "Aeronaves (flota)":
                    await new AircraftMenu(_provider).MostrarAsync();
                    break;

                case "Asientos de aeronave":
                    await new AircraftSeatMenu(_provider).MostrarAsync();
                    break;

                case "Volver":
                    return;
            }
        }
    }
}
