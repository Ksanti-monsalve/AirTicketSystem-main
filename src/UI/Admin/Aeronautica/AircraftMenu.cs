// src/UI/Admin/Aeronautica/AircraftMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraft.Application.UseCases;
using AirTicketSystem.modules.aircraftmodel.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AircraftMenu
{
    private readonly IServiceProvider _provider;

    public AircraftMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Aeronaves (Aviones)");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todos",
                    "Listar disponibles",
                    "Registrar nueva aeronave",
                    "Editar fechas",
                    "Enviar a mantenimiento",
                    "Registrar mantenimiento completado",
                    "Dar de baja",
                    "Reactivar aeronave",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todos":                       await ListarTodosAsync();         break;
                case "Listar disponibles":                 await ListarDisponiblesAsync();   break;
                case "Registrar nueva aeronave":           await CrearAsync();               break;
                case "Editar fechas":                      await EditarAsync();              break;
                case "Enviar a mantenimiento":             await EnviarMantenimientoAsync(); break;
                case "Registrar mantenimiento completado": await RegistrarMantenimientoAsync(); break;
                case "Dar de baja":                        await DarDeBajaAsync();           break;
                case "Reactivar aeronave":                 await ReactivarAsync();           break;
                case "Volver":                             return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllAircraftUseCase>().ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin aeronaves registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Matrícula", "Estado", "Activo", "ModeloID", "AerolineaID", "Horas Vuelo");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla,
                    a.Id.ToString(), a.Matricula.Valor, a.Estado.Valor,
                    a.Activo.Valor ? "Sí" : "No",
                    a.ModeloAvionId.ToString(), a.AerolineaId.ToString(),
                    a.TotalHorasVuelo.Valor.ToString("F1"));

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarDisponiblesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAvailableAircraftUseCase>().ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay aeronaves disponibles."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Matrícula", "ModeloID", "AerolineaID");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla,
                    a.Id.ToString(), a.Matricula.Valor,
                    a.ModeloAvionId.ToString(), a.AerolineaId.ToString());

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Registrar Nueva Aeronave");

        await MostrarModelosAsync();

        var modeloId   = SpectreHelper.PedirEntero("ID del modelo de aeronave");
        var aerolineaId = SpectreHelper.PedirEntero("ID de la aerolínea");
        var matricula  = SpectreHelper.PedirTexto("Matrícula (ej: HK-1234)");

        var fabricacionStr  = SpectreHelper.PedirTexto("Fecha de fabricación dd/MM/yyyy (opcional)");
        var mantenimientoStr = SpectreHelper.PedirTexto("Próximo mantenimiento dd/MM/yyyy (opcional)");

        DateOnly? fabricacion   = DateOnly.TryParseExact(fabricacionStr,  "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var f) ? f : null;
        DateOnly? mantenimiento = DateOnly.TryParseExact(mantenimientoStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var m) ? m : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<CreateAircraftUseCase>()
                .ExecuteAsync(modeloId, aerolineaId, matricula, fabricacion, mantenimiento);
            SpectreHelper.MostrarExito($"Aeronave '{r.Matricula.Valor}' registrada (ID {r.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Fechas de Aeronave");
        var id = SpectreHelper.PedirEntero("ID de la aeronave");

        var fabricacionStr   = SpectreHelper.PedirTexto("Fecha fabricación dd/MM/yyyy (opcional)");
        var mantenimientoStr = SpectreHelper.PedirTexto("Próximo mantenimiento dd/MM/yyyy (opcional)");

        DateOnly? fabricacion   = DateOnly.TryParseExact(fabricacionStr,  "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var f) ? f : null;
        DateOnly? mantenimiento = DateOnly.TryParseExact(mantenimientoStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var m) ? m : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<UpdateAircraftUseCase>()
                .ExecuteAsync(id, fabricacion, mantenimiento);
            SpectreHelper.MostrarExito($"Aeronave '{r.Matricula.Valor}' actualizada.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EnviarMantenimientoAsync()
    {
        SpectreHelper.MostrarSubtitulo("Enviar a Mantenimiento");
        var id               = SpectreHelper.PedirEntero("ID de la aeronave");
        var proximoMantStr   = SpectreHelper.PedirTexto("Fecha próximo mantenimiento dd/MM/yyyy");

        if (!DateOnly.TryParseExact(proximoMantStr, "dd/MM/yyyy", null,
            System.Globalization.DateTimeStyles.None, out var proximoMant))
        {
            SpectreHelper.MostrarError("Formato de fecha inválido."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<SendToMaintenanceUseCase>().ExecuteAsync(id, proximoMant);
            SpectreHelper.MostrarExito("Aeronave enviada a mantenimiento.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task RegistrarMantenimientoAsync()
    {
        SpectreHelper.MostrarSubtitulo("Registrar Mantenimiento Completado");
        var id             = SpectreHelper.PedirEntero("ID de la aeronave");
        var proximoMantStr = SpectreHelper.PedirTexto("Fecha próximo mantenimiento dd/MM/yyyy");

        if (!DateOnly.TryParseExact(proximoMantStr, "dd/MM/yyyy", null,
            System.Globalization.DateTimeStyles.None, out var proximoMant))
        {
            SpectreHelper.MostrarError("Formato de fecha inválido."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<RegisterMaintenanceUseCase>().ExecuteAsync(id, proximoMant);
            SpectreHelper.MostrarExito("Mantenimiento registrado. Aeronave disponible.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task DarDeBajaAsync()
    {
        SpectreHelper.MostrarSubtitulo("Dar de Baja Aeronave");
        var id = SpectreHelper.PedirEntero("ID de la aeronave");

        if (!SpectreHelper.Confirmar("¿Confirma dar de baja esta aeronave?"))
        { SpectreHelper.MostrarInfo("Cancelado."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<DecommissionAircraftUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Aeronave dada de baja.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task ReactivarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Reactivar Aeronave");
        var id = SpectreHelper.PedirEntero("ID de la aeronave");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<ReactivateAircraftUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Aeronave reactivada.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarModelosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllAircraftModelsUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Modelo", "Código", "FabricanteID");
            foreach (var m in lista)
                SpectreHelper.AgregarFila(tabla, m.Id.ToString(), m.Nombre.Valor,
                    m.CodigoModelo.Valor, m.FabricanteId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
