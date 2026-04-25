// src/UI/Admin/ClientesUsuarios/ClientesUsuariosMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.UI.Admin.Billing;

namespace AirTicketSystem.UI.Admin.ClientesUsuarios;

public sealed class ClientesUsuariosMenu
{
    private readonly IServiceProvider _provider;

    public ClientesUsuariosMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Gestión de Clientes y Usuarios");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "Clientes",
                    "Usuarios",
                    "Personas",
                    "Log de accesos",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Clientes":      await new ClientMenu(_provider).MostrarAsync();      break;
                case "Usuarios":      await new UserAdminMenu(_provider).MostrarAsync();   break;
                case "Personas":      await new PersonasMenu(_provider).MostrarAsync();    break;
                case "Log de accesos": await new AccessLogMenu(_provider).MostrarAsync();  break;
                case "Volver":        return;
            }
        }
    }
}
