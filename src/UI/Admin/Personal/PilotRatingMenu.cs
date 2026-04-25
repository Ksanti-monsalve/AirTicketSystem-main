// src/UI/Admin/Personal/PilotRatingMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.pilotrating.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Personal;

public sealed class PilotRatingMenu
{
    private readonly IServiceProvider _provider;

    public PilotRatingMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Habilitaciones de Piloto");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Habilitaciones por licencia",
                    "Habilitaciones por modelo de avión",
                    "Habilitaciones vigentes",
                    "Ver habilitación por ID",
                    "Crear habilitación",
                    "Renovar habilitación",
                    "Eliminar habilitación",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Habilitaciones por licencia":        await ListarPorLicenciaAsync();      break;
                case "Habilitaciones por modelo de avión": await ListarPorModeloAsync();        break;
                case "Habilitaciones vigentes":            await ListarVigentesAsync();         break;
                case "Ver habilitación por ID":            await VerPorIdAsync();              break;
                case "Crear habilitación":                 await CrearAsync();                  break;
                case "Renovar habilitación":               await RenovarAsync();                break;
                case "Eliminar habilitación":              await EliminarAsync();               break;
                case "Volver":                             return;
            }
        }
    }

    private async Task ListarPorLicenciaAsync()
    {
        var licenciaId = SpectreHelper.PedirEntero("ID de la licencia");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetRatingsByLicenseUseCase>().ExecuteAsync(licenciaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin habilitaciones para esta licencia."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "ModeloAvionID", "Habilitación", "Vencimiento", "Días restantes", "Vigente");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.ModeloAvionId.ToString(),
                    r.FechaHabilitacion.Valor.ToString("yyyy-MM-dd"),
                    r.FechaVencimiento.Valor.ToString("yyyy-MM-dd"),
                    r.DiasHastaVencimiento.ToString(),
                    r.EstaVigente ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorModeloAsync()
    {
        var modeloId = SpectreHelper.PedirEntero("ID del modelo de avión");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetRatingsByAircraftModelUseCase>().ExecuteAsync(modeloId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin habilitaciones para este modelo."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "LicenciaID", "Habilitación", "Vencimiento", "Vigente");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.LicenciaId.ToString(),
                    r.FechaHabilitacion.Valor.ToString("yyyy-MM-dd"),
                    r.FechaVencimiento.Valor.ToString("yyyy-MM-dd"),
                    r.EstaVigente ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarVigentesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetVigenteRatingsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin habilitaciones vigentes."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "LicenciaID", "ModeloID", "Vencimiento", "Días restantes");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.LicenciaId.ToString(), r.ModeloAvionId.ToString(),
                    r.FechaVencimiento.Valor.ToString("yyyy-MM-dd"), r.DiasHastaVencimiento.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la habilitación");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<GetPilotRatingByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",             r.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "LicenciaID",     r.LicenciaId.ToString());
            SpectreHelper.AgregarFila(tabla, "ModeloAvionID",  r.ModeloAvionId.ToString());
            SpectreHelper.AgregarFila(tabla, "Habilitación",   r.FechaHabilitacion.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Vencimiento",    r.FechaVencimiento.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Días restantes", r.DiasHastaVencimiento.ToString());
            SpectreHelper.AgregarFila(tabla, "Vigente",        r.EstaVigente ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Habilitación");
        var licenciaId       = SpectreHelper.PedirEntero("ID de la licencia");
        var modeloAvionId    = SpectreHelper.PedirEntero("ID del modelo de avión");
        var habilitacionStr  = SpectreHelper.PedirTexto("Fecha de habilitación (yyyy-MM-dd)");
        var vencimientoStr   = SpectreHelper.PedirTexto("Fecha de vencimiento (yyyy-MM-dd)");

        if (!DateOnly.TryParse(habilitacionStr, out var habilitacion) || !DateOnly.TryParse(vencimientoStr, out var vencimiento))
        {
            SpectreHelper.MostrarError("Fechas inválidas."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreatePilotRatingUseCase>()
                .ExecuteAsync(licenciaId, modeloAvionId, habilitacion, vencimiento);
            SpectreHelper.MostrarExito($"Habilitación creada (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RenovarAsync()
    {
        var id             = SpectreHelper.PedirEntero("ID de la habilitación");
        var vencimientoStr = SpectreHelper.PedirTexto("Nueva fecha de vencimiento (yyyy-MM-dd)");
        if (!DateOnly.TryParse(vencimientoStr, out var vencimiento)) { SpectreHelper.MostrarError("Fecha inválida."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RenewPilotRatingUseCase>().ExecuteAsync(id, vencimiento);
            SpectreHelper.MostrarExito("Habilitación renovada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la habilitación a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePilotRatingUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Habilitación eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
