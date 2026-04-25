// src/UI/Admin/Flights/FlightModuleMenu.cs
using AirTicketSystem.shared.UI;

namespace AirTicketSystem.UI.Admin.Flights;

public sealed class FlightModuleMenu
{
    private readonly IServiceProvider _provider;

    public FlightModuleMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Gestión de Vuelos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "4.1 Vuelos (CRUD + cambio estado)",
                    "4.2 Tripulación",
                    "4.3 Disponibilidad de asientos",
                    "4.4 Historial de vuelos",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "4.1 Vuelos (CRUD + cambio estado)": await new FlightMenu(_provider).MostrarAsync();       break;
                case "4.2 Tripulación":                   await new FlightCrewMenu(_provider).MostrarAsync();   break;
                case "4.3 Disponibilidad de asientos":    await new SeatAvailabilityMenu(_provider).MostrarAsync(); break;
                case "4.4 Historial de vuelos":           await new FlightHistoryMenu(_provider).MostrarAsync(); break;
                case "Volver":                            return;
            }
        }
    }
}
