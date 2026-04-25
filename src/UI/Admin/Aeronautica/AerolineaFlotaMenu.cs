// src/UI/Admin/Aeronautica/AerolineaFlotaMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.UI.Admin.AirportsRoutes;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AerolineaFlotaMenu
{
    private readonly IServiceProvider _provider;

    public AerolineaFlotaMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Gestión de Aerolíneas y Flota");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "2.1 Aerolíneas",
                    "2.2 Aeropuertos",
                    "2.3 Terminales",
                    "2.4 Puertas de embarque",
                    "2.5 Rutas",
                    "2.6 Fabricantes de avión",
                    "2.7 Modelos de avión",
                    "2.8 Aviones",
                    "2.9 Asientos de avión",
                    "2.10 Clases de servicio",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "2.1 Aerolíneas":           await new AirlineMenu(_provider).MostrarAsync();              break;
                case "2.2 Aeropuertos":          await new AirportMenu(_provider).MostrarAsync();              break;
                case "2.3 Terminales":           await new TerminalMenu(_provider).MostrarAsync();             break;
                case "2.4 Puertas de embarque":  await new GateMenu(_provider).MostrarAsync();                 break;
                case "2.5 Rutas":                await new RouteMenu(_provider).MostrarAsync();                break;
                case "2.6 Fabricantes de avión": await new AircraftManufacturerMenu(_provider).MostrarAsync(); break;
                case "2.7 Modelos de avión":     await new AircraftModelMenu(_provider).MostrarAsync();        break;
                case "2.8 Aviones":              await new AircraftMenu(_provider).MostrarAsync();             break;
                case "2.9 Asientos de avión":    await new AircraftSeatMenu(_provider).MostrarAsync();         break;
                case "2.10 Clases de servicio":  await new ServiceClassMenuWrapper(_provider).MostrarAsync();  break;
                case "Volver":                   return;
            }
        }
    }
}
