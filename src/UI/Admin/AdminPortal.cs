// src/UI/Admin/AdminPortal.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.UI.Admin.GeoConfig;
using AirTicketSystem.UI.Admin.Aeronautica;
using AirTicketSystem.UI.Admin.AirportsRoutes;
using AirTicketSystem.UI.Admin.Personal;
using AirTicketSystem.UI.Admin.Flights;
using AirTicketSystem.UI.Admin.ClientesUsuarios;
using AirTicketSystem.UI.Admin.Reservations;
using AirTicketSystem.UI.Admin.Billing;
using AirTicketSystem.UI.Admin.Reportes;

namespace AirTicketSystem.UI.Admin;

public sealed class AdminPortal
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public AdminPortal(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo(
                $"Portal Administrativo  —  {_session.CurrentUserName}");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "Seleccione un módulo",
                [
                    "1. Gestión Geográfica",
                    "2. Gestión de Aerolíneas y Flota",
                    "3. Gestión de Personal",
                    "4. Gestión de Vuelos",
                    "5. Gestión de Clientes y Usuarios",
                    "6. Gestión de Reservas y Pagos",
                    "7. Gestión de Catálogos",
                    "8. Reportes LINQ",
                    "9. Cerrar sesión"
                ]);

            switch (opcion)
            {
                case "1. Gestión Geográfica":
                    await new GeoConfigMenu(_provider).MostrarAsync();
                    break;

                case "2. Gestión de Aerolíneas y Flota":
                    await new AerolineaFlotaMenu(_provider).MostrarAsync();
                    break;

                case "3. Gestión de Personal":
                    await new PersonalMenu(_provider).MostrarAsync();
                    break;

                case "4. Gestión de Vuelos":
                    await new FlightModuleMenu(_provider).MostrarAsync();
                    break;

                case "5. Gestión de Clientes y Usuarios":
                    await new ClientesUsuariosMenu(_provider).MostrarAsync();
                    break;

                case "6. Gestión de Reservas y Pagos":
                    await new ReservasYPagosMenu(_provider, _session).MostrarAsync();
                    break;

                case "7. Gestión de Catálogos":
                    await new CatalogosMenu(_provider).MostrarAsync();
                    break;

                case "8. Reportes LINQ":
                    await new ReportesMenu(_provider).MostrarAsync();
                    break;

                case "9. Cerrar sesión":
                    _session.Logout();
                    return;
            }
        }
    }
}
