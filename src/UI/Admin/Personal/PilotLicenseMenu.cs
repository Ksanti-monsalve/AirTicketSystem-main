// src/UI/Admin/Personal/PilotLicenseMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.pilotlicense.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Personal;

public sealed class PilotLicenseMenu
{
    private readonly IServiceProvider _provider;

    public PilotLicenseMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Licencias de Piloto");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Licencias por trabajador",
                    "Ver licencia por ID",
                    "Licencias vigentes",
                    "Licencias por vencer",
                    "Crear licencia",
                    "Renovar licencia",
                    "Suspender licencia",
                    "Reactivar licencia",
                    "Eliminar licencia",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Licencias por trabajador": await ListarPorTrabajadorAsync();  break;
                case "Ver licencia por ID":      await VerPorIdAsync();             break;
                case "Licencias vigentes":       await ListarVigentesAsync();       break;
                case "Licencias por vencer":     await ListarPorVencerAsync();      break;
                case "Crear licencia":           await CrearAsync();                break;
                case "Renovar licencia":         await RenovarAsync();              break;
                case "Suspender licencia":       await SuspenderAsync();            break;
                case "Reactivar licencia":       await ReactivarAsync();            break;
                case "Eliminar licencia":        await EliminarAsync();             break;
                case "Volver":                   return;
            }
        }
    }

    private async Task ListarPorTrabajadorAsync()
    {
        var trabajadorId = SpectreHelper.PedirEntero("ID del trabajador");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetLicensesByWorkerUseCase>().ExecuteAsync(trabajadorId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin licencias para este trabajador."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Número", "Tipo", "Expedición", "Vencimiento", "Autoridad", "Activa");
            foreach (var l in lista)
                SpectreHelper.AgregarFila(tabla,
                    l.Id.ToString(), l.NumeroLicencia.Valor, l.TipoLicencia.Valor,
                    l.FechaExpedicion.Valor.ToString("yyyy-MM-dd"),
                    l.FechaVencimiento.Valor.ToString("yyyy-MM-dd"),
                    l.AutoridadEmisora.Valor, l.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la licencia");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<GetPilotLicenseByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",             l.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "TrabajadorID",   l.TrabajadorId.ToString());
            SpectreHelper.AgregarFila(tabla, "Número",         l.NumeroLicencia.Valor);
            SpectreHelper.AgregarFila(tabla, "Tipo",           l.TipoLicencia.Valor);
            SpectreHelper.AgregarFila(tabla, "Expedición",     l.FechaExpedicion.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Vencimiento",    l.FechaVencimiento.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Días restantes", l.DiasHastaVencimiento.ToString());
            SpectreHelper.AgregarFila(tabla, "Autoridad",      l.AutoridadEmisora.Valor);
            SpectreHelper.AgregarFila(tabla, "Activa",         l.Activa.Valor ? "Sí" : "No");
            SpectreHelper.AgregarFila(tabla, "Vuelos comerciales", l.HabilitaVuelosComerciales ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarVigentesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetVigenteLicensesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin licencias vigentes."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "TrabajadorID", "Número", "Tipo", "Vencimiento", "Días restantes");
            foreach (var l in lista)
                SpectreHelper.AgregarFila(tabla,
                    l.Id.ToString(), l.TrabajadorId.ToString(), l.NumeroLicencia.Valor,
                    l.TipoLicencia.Valor, l.FechaVencimiento.Valor.ToString("yyyy-MM-dd"),
                    l.DiasHastaVencimiento.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorVencerAsync()
    {
        var dias = SpectreHelper.PedirEntero("Días de anticipación (ej: 30)");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetLicensesExpiringSoonUseCase>().ExecuteAsync(dias);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin licencias por vencer en {dias} días."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "TrabajadorID", "Número", "Tipo", "Vencimiento", "Días restantes");
            foreach (var l in lista)
                SpectreHelper.AgregarFila(tabla,
                    l.Id.ToString(), l.TrabajadorId.ToString(), l.NumeroLicencia.Valor,
                    l.TipoLicencia.Valor, l.FechaVencimiento.Valor.ToString("yyyy-MM-dd"),
                    l.DiasHastaVencimiento.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Licencia de Piloto");
        var trabajadorId    = SpectreHelper.PedirEntero("ID del trabajador");
        var numero          = SpectreHelper.PedirTexto("Número de licencia");
        var tipo            = SpectreHelper.PedirTexto("Tipo (PPL, CPL, ATPL)");
        var expedicionStr   = SpectreHelper.PedirTexto("Fecha de expedición (yyyy-MM-dd)");
        var vencimientoStr  = SpectreHelper.PedirTexto("Fecha de vencimiento (yyyy-MM-dd)");
        var autoridad       = SpectreHelper.PedirTexto("Autoridad emisora");

        if (!DateOnly.TryParse(expedicionStr, out var expedicion) || !DateOnly.TryParse(vencimientoStr, out var vencimiento))
        {
            SpectreHelper.MostrarError("Fechas inválidas."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<CreatePilotLicenseUseCase>()
                .ExecuteAsync(trabajadorId, numero, tipo, expedicion, vencimiento, autoridad);
            SpectreHelper.MostrarExito($"Licencia '{l.NumeroLicencia.Valor}' creada (ID {l.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RenovarAsync()
    {
        var id             = SpectreHelper.PedirEntero("ID de la licencia");
        var vencimientoStr = SpectreHelper.PedirTexto("Nueva fecha de vencimiento (yyyy-MM-dd)");
        if (!DateOnly.TryParse(vencimientoStr, out var vencimiento)) { SpectreHelper.MostrarError("Fecha inválida."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RenewPilotLicenseUseCase>().ExecuteAsync(id, vencimiento);
            SpectreHelper.MostrarExito("Licencia renovada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task SuspenderAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la licencia a suspender");
        if (!SpectreHelper.Confirmar("¿Confirma suspender la licencia?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SuspendPilotLicenseUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Licencia suspendida.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ReactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la licencia a reactivar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ReactivatePilotLicenseUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Licencia reactivada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la licencia a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePilotLicenseUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Licencia eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
