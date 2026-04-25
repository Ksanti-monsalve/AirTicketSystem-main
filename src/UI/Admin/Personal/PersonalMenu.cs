// src/UI/Admin/Personal/PersonalMenu.cs
using AirTicketSystem.shared.UI;

namespace AirTicketSystem.UI.Admin.Personal;

public sealed class PersonalMenu
{
    private readonly IServiceProvider _provider;

    public PersonalMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Gestión de Personal");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "Trabajadores",
                    "Licencias de piloto",
                    "Habilitaciones",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Trabajadores":        await new WorkerMenu(_provider).MostrarAsync();       break;
                case "Licencias de piloto": await new PilotLicenseMenu(_provider).MostrarAsync(); break;
                case "Habilitaciones":      await new PilotRatingMenu(_provider).MostrarAsync();  break;
                case "Volver":              return;
            }
        }
    }
}
