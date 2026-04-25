// src/UI/Admin/AirportsRoutes/AirportsRoutesMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class AirportsRoutesMenu
{
    private readonly IServiceProvider _provider;

    public AirportsRoutesMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Aeropuertos y Rutas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "Aeropuertos",
                    "Terminales",
                    "Puertas de embarque",
                    "Aerolíneas",
                    "Rutas",
                    "Tarifas",
                    "Restricciones de equipaje",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Aeropuertos":
                    await new AirportMenu(_provider).MostrarAsync();
                    break;
                case "Terminales":
                    await new TerminalMenu(_provider).MostrarAsync();
                    break;
                case "Puertas de embarque":
                    await new GateMenu(_provider).MostrarAsync();
                    break;
                case "Aerolíneas":
                    await new AirlineMenu(_provider).MostrarAsync();
                    break;
                case "Rutas":
                    await new RouteMenu(_provider).MostrarAsync();
                    break;
                case "Tarifas":
                    await new FareMenu(_provider).MostrarAsync();
                    break;
                case "Restricciones de equipaje":
                    await new LuggageRestrictionMenu(_provider).MostrarAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }
}
