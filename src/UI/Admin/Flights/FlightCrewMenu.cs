// src/UI/Admin/Flights/FlightCrewMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flightcrew.Application.UseCases;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.aggregate;

namespace AirTicketSystem.UI.Admin.Flights;

public sealed class FlightCrewMenu
{
    private readonly IServiceProvider _provider;

    public FlightCrewMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Tripulación de Vuelo");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver tripulación de vuelo",
                    "Validar tripulación de vuelo",
                    "Asignar miembro de tripulación",
                    "Remover miembro de tripulación",
                    "Eliminar asignación",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver tripulación de vuelo":      await VerTripulacionAsync();    break;
                case "Validar tripulación de vuelo":  await ValidarTripulacionAsync(); break;
                case "Asignar miembro de tripulación": await AsignarAsync();          break;
                case "Remover miembro de tripulación": await RemoverAsync();          break;
                case "Eliminar asignación":           await EliminarAsync();          break;
                case "Volver":                        return;
            }
        }
    }

    private async Task VerTripulacionAsync()
    {
        var vuelo = await SelectorUI.SeleccionarVueloAsync(_provider);
        if (vuelo is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var vueloId = vuelo.Id;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetCrewByFlightUseCase>().ExecuteAsync(vueloId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin tripulación asignada a este vuelo."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "TrabajadorID", "Rol en vuelo");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.TrabajadorId.ToString(), c.RolEnVuelo.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ValidarTripulacionAsync()
    {
        var vuelo = await SelectorUI.SeleccionarVueloAsync(_provider);
        if (vuelo is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var vueloId = vuelo.Id;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var resultado = await scope.ServiceProvider.GetRequiredService<ValidateFlightCrewUseCase>().ExecuteAsync(vueloId);
            if (resultado)
                SpectreHelper.MostrarExito("Tripulación válida para operar el vuelo.");
            else
                SpectreHelper.MostrarAdvertencia("La tripulación está incompleta o tiene problemas.");
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task AsignarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Asignar Miembro de Tripulación");
        var vuelo = await SelectorUI.SeleccionarVueloAsync(_provider);
        if (vuelo is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var vueloId = vuelo.Id;

        var trabajador = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (trabajador is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var trabajadorId = trabajador.Id;

        var rol          = SpectreHelper.PedirTexto("Rol (PILOTO, COPILOTO, AUXILIAR_CABINA, AUXILIAR_CARGA)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<AssignCrewMemberUseCase>()
                .ExecuteAsync(vueloId, trabajadorId, rol);
            SpectreHelper.MostrarExito($"Trabajador #{c.TrabajadorId} asignado como {c.RolEnVuelo.Valor} al vuelo #{c.VueloId}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RemoverAsync()
    {
        var flightCrewId = SpectreHelper.PedirEntero("ID de la asignación de tripulación (FlightCrewID)");
        if (!SpectreHelper.Confirmar("¿Confirma remover al miembro de tripulación?")) { SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RemoveCrewMemberUseCase>().ExecuteAsync(flightCrewId);
            SpectreHelper.MostrarExito("Miembro removido de la tripulación.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var crewId = SpectreHelper.PedirEntero("ID de la asignación de tripulación");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar la asignación?")) { SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteFlightCrewUseCase>().ExecuteAsync(crewId);
            SpectreHelper.MostrarExito("Asignación eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
