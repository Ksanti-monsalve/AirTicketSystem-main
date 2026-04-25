// src/UI/Admin/GeoConfig/GeoConfigMenu.cs
using AirTicketSystem.shared.UI;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class GeoConfigMenu
{
    private readonly IServiceProvider _provider;

    public GeoConfigMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Configuración Geográfica");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "Seleccione un módulo",
                [
                    "Continentes",
                    "Países",
                    "Regiones",
                    "Departamentos / Provincias",
                    "Ciudades",
                    "Catálogos del sistema",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Continentes":
                    await new ContinentMenu(_provider).MostrarAsync();
                    break;

                case "Países":
                    await new CountryMenu(_provider).MostrarAsync();
                    break;

                case "Regiones":
                    await new RegionMenu(_provider).MostrarAsync();
                    break;

                case "Departamentos / Provincias":
                    await new DepartmentMenu(_provider).MostrarAsync();
                    break;

                case "Ciudades":
                    await new CityMenu(_provider).MostrarAsync();
                    break;

                case "Catálogos del sistema":
                    await new CatalogosMenu(_provider).MostrarAsync();
                    break;

                case "Volver":
                    return;
            }
        }
    }
}
