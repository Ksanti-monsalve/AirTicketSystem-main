// src/UI/Admin/AirportsRoutes/LuggageRestrictionMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.luggagerestriction.Application.UseCases;
using AirTicketSystem.modules.fare.Application.UseCases;
using AirTicketSystem.modules.luggagetype.Application.UseCases;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class LuggageRestrictionMenu
{
    private readonly IServiceProvider _provider;

    public LuggageRestrictionMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Restricciones de Equipaje");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas",
                    "Listar por tarifa",
                    "Crear",
                    "Editar",
                    "Eliminar",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":    await ListarTodasAsync();   break;
                case "Listar por tarifa": await ListarPorTarifaAsync(); break;
                case "Crear":           await CrearAsync();         break;
                case "Editar":          await EditarAsync();        break;
                case "Eliminar":        await EliminarAsync();      break;
                case "Volver":          return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllRestrictionsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin restricciones."); SpectreHelper.EsperarTecla(); return; }

            MostrarTablaRestricciones(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorTarifaAsync()
    {
        var tarifaId = SpectreHelper.PedirEntero("ID de la tarifa");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetRestrictionsByFareUseCase>()
                .ExecuteAsync(tarifaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin restricciones para esa tarifa."); SpectreHelper.EsperarTecla(); return; }

            MostrarTablaRestricciones(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Restricción de Equipaje");
        await MostrarTarifasAsync();
        var tarifaId = SpectreHelper.PedirEntero("ID de la tarifa");

        var tipoEquipaje = await SelectorUI.SeleccionarTipoEquipajeAsync(_provider);
        if (tipoEquipaje is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var tipoEquipajeId = tipoEquipaje.Id;

        var piezas      = SpectreHelper.PedirEntero("Piezas incluidas");
        var pesoMax     = SpectreHelper.PedirDecimal("Peso máximo en kg");
        var costoExceso = SpectreHelper.PedirDecimal("Costo por kg de exceso");

        var largoStr = SpectreHelper.PedirTexto("Largo máximo en cm (opcional)");
        var anchoStr = SpectreHelper.PedirTexto("Ancho máximo en cm (opcional)");
        var altoStr  = SpectreHelper.PedirTexto("Alto máximo en cm (opcional)");
        int? largo = int.TryParse(largoStr, out var lv) && lv > 0 ? lv : null;
        int? ancho = int.TryParse(anchoStr, out var av) && av > 0 ? av : null;
        int? alto  = int.TryParse(altoStr,  out var hv) && hv > 0 ? hv : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateLuggageRestrictionUseCase>()
                .ExecuteAsync(tarifaId, tipoEquipajeId, piezas, pesoMax, costoExceso, largo, ancho, alto);
            SpectreHelper.MostrarExito($"Restricción creada (ID {r.Id}). Máx {r.PesoMaximoKg.Valor} kg, {r.PiezasIncluidas.Valor} piezas.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Restricción de Equipaje");
        var id = SpectreHelper.PedirEntero("ID de la restricción");

        var piezas      = SpectreHelper.PedirEntero("Piezas incluidas");
        var pesoMax     = SpectreHelper.PedirDecimal("Peso máximo en kg");
        var costoExceso = SpectreHelper.PedirDecimal("Costo por kg de exceso");

        var largoStr = SpectreHelper.PedirTexto("Largo máximo en cm (opcional)");
        var anchoStr = SpectreHelper.PedirTexto("Ancho máximo en cm (opcional)");
        var altoStr  = SpectreHelper.PedirTexto("Alto máximo en cm (opcional)");
        int? largo = int.TryParse(largoStr, out var lv) && lv > 0 ? lv : null;
        int? ancho = int.TryParse(anchoStr, out var av) && av > 0 ? av : null;
        int? alto  = int.TryParse(altoStr,  out var hv) && hv > 0 ? hv : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateLuggageRestrictionUseCase>()
                .ExecuteAsync(id, piezas, pesoMax, costoExceso, largo, ancho, alto);
            SpectreHelper.MostrarExito($"Restricción {r.Id} actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la restricción a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteLuggageRestrictionUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Restricción eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private static void MostrarTablaRestricciones(
        IEnumerable<AirTicketSystem.modules.luggagerestriction.Domain.aggregate.LuggageRestriction> lista)
    {
        var tabla = SpectreHelper.CrearTabla("ID", "TarifaID", "TipoEquipajeID", "Piezas", "PesoMáx(kg)", "CostoExceso", "Largo", "Ancho", "Alto");
        foreach (var r in lista)
            SpectreHelper.AgregarFila(tabla,
                r.Id.ToString(), r.TarifaId.ToString(), r.TipoEquipajeId.ToString(),
                r.PiezasIncluidas.Valor.ToString(),
                r.PesoMaximoKg.Valor.ToString("F1"),
                r.CostoExcesoKg.Valor.ToString("C2"),
                r.LargoMaxCm?.Valor.ToString() ?? "-",
                r.AnchoMaxCm?.Valor.ToString() ?? "-",
                r.AltoMaxCm?.Valor.ToString() ?? "-");
        SpectreHelper.MostrarTabla(tabla);
    }

    private async Task MostrarTarifasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveFaresUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "RutaID", "ClaseID", "Nombre", "Total");
            foreach (var f in lista)
                SpectreHelper.AgregarFila(tabla,
                    f.Id.ToString(), f.RutaId.ToString(), f.ClaseServicioId.ToString(),
                    f.Nombre.Valor, f.PrecioTotal.Valor.ToString("C2"));
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarTiposEquipajeAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllLuggageTypesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Tipo");
            foreach (var t in lista)
                SpectreHelper.AgregarFila(tabla, t.Id.ToString(), t.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
