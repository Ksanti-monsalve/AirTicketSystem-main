// src/UI/Client/ClientPortal.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.UI.Client;

public sealed class ClientPortal
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public ClientPortal(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo(
                $"Portal Cliente  —  {_session.CurrentUserName}");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "¿Qué desea hacer?",
                [
                    "1. Buscar vuelos y reservar",
                    "2. Mis reservas",
                    "3. Mi perfil",
                    "Cerrar sesión"
                ]);

            if (opcion == "Cerrar sesión")
            {
                _session.Logout();
                return;
            }

            switch (opcion)
            {
                case "1. Buscar vuelos y reservar":
                    await new FlightSearchMenu(_provider, _session).MostrarAsync();
                    break;

                case "2. Mis reservas":
                    await new MyBookingsMenu(_provider, _session).MostrarAsync();
                    break;

                case "3. Mi perfil":
                    await new MyProfileMenu(_provider, _session).MostrarAsync();
                    break;
            }
        }
    }
}
