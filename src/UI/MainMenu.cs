// src/UI/MainMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.UI.Admin;
using AirTicketSystem.UI.Client;

namespace AirTicketSystem.UI;

public sealed class MainMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public MainMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task EnrutarAsync()
    {
        if (!_session.IsAuthenticated)
            throw new InvalidOperationException(
                "Se intentó enrutar sin sesión activa.");

        if (_session.IsAdmin)
        {
            var portal = new AdminPortal(_provider, _session);
            await portal.MostrarAsync();
        }
        else if (_session.IsClient)
        {
            var portal = new ClientPortal(_provider, _session);
            await portal.MostrarAsync();
        }
        else
        {
            SpectreHelper.MostrarError(
                $"Rol '{_session.CurrentRole}' no tiene portal asignado.");
            SpectreHelper.EsperarTecla();
            _session.Logout();
        }
    }
}
