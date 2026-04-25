// src/UI/Admin/Reservations/LuggageAdminMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.luggage.Application.UseCases;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class LuggageAdminMenu
{
    private readonly IServiceProvider _provider;

    public LuggageAdminMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Equipaje");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver equipaje de pasajero",
                    "Registrar equipaje",
                    "Registrar equipaje en check-in",
                    "Reportar equipaje dañado",
                    "Reportar equipaje perdido",
                    "Enviar a bodega",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver equipaje de pasajero":      await VerPorPasajeroAsync();      break;
                case "Registrar equipaje":            await RegistrarAsync();            break;
                case "Registrar equipaje en check-in": await RegistrarCheckinAsync();   break;
                case "Reportar equipaje dañado":      await ReportarDañadoAsync();      break;
                case "Reportar equipaje perdido":     await ReportarPerdidoAsync();     break;
                case "Enviar a bodega":               await EnviarBodegaAsync();        break;
                case "Volver":                        return;
            }
        }
    }

    private async Task VerPorPasajeroAsync()
    {
        var pasajeroId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetLuggageByPassengerUseCase>().ExecuteAsync(pasajeroId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin equipaje registrado para este pasajero."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "TipoEquipaje", "Peso(kg)", "Estado", "Descripción");
            foreach (var l in lista)
                SpectreHelper.AgregarFila(tabla,
                    l.Id.ToString(), l.TipoEquipajeId.ToString(),
                    l.PesoDeclaradoKg?.Valor.ToString("F1") ?? "-",
                    l.Estado.Valor,
                    l.Descripcion?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task RegistrarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Registrar Equipaje");
        var pasajeroId   = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        var vueloId      = SpectreHelper.PedirEntero("ID del vuelo");
        var tipoEquip = await SelectorUI.SeleccionarTipoEquipajeAsync(_provider);
        if (tipoEquip is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var tipoEquipId  = tipoEquip.Id;
        var descripcion  = SpectreHelper.PedirTexto("Descripción (opcional)");
        var pesoStr      = SpectreHelper.PedirTexto("Peso declarado en kg (opcional)");

        string? desc    = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;
        decimal? peso   = decimal.TryParse(pesoStr, out var p) ? p : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<RegisterLuggageUseCase>()
                .ExecuteAsync(pasajeroId, vueloId, tipoEquipId, desc, peso);
            SpectreHelper.MostrarExito($"Equipaje registrado (ID {l.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RegistrarCheckinAsync()
    {
        var luggageId        = SpectreHelper.PedirEntero("ID del equipaje");
        var pesoRealStr      = SpectreHelper.PedirTexto("Peso real en kg");
        var pesoMaxStr       = SpectreHelper.PedirTexto("Peso máximo permitido en kg");
        var costoExcStr      = SpectreHelper.PedirTexto("Costo por kg excedido");

        if (!decimal.TryParse(pesoRealStr, out var pesoReal) ||
            !decimal.TryParse(pesoMaxStr, out var pesoMax) ||
            !decimal.TryParse(costoExcStr, out var costoExc))
        {
            SpectreHelper.MostrarError("Valores inválidos."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<ProcessCheckInLuggageUseCase>()
                .ExecuteAsync(luggageId, pesoReal, pesoMax, costoExc);
            SpectreHelper.MostrarExito($"Equipaje #{l.Id} procesado en check-in. Estado: {l.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ReportarDañadoAsync()
    {
        var luggageId = SpectreHelper.PedirEntero("ID del equipaje");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<ReportDamagedLuggageUseCase>().ExecuteAsync(luggageId);
            SpectreHelper.MostrarExito($"Equipaje #{l.Id} reportado como dañado. Estado: {l.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ReportarPerdidoAsync()
    {
        var luggageId = SpectreHelper.PedirEntero("ID del equipaje");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<ReportLostLuggageUseCase>().ExecuteAsync(luggageId);
            SpectreHelper.MostrarExito($"Equipaje #{l.Id} reportado como perdido.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EnviarBodegaAsync()
    {
        var luggageId = SpectreHelper.PedirEntero("ID del equipaje");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var l = await scope.ServiceProvider.GetRequiredService<SendToBaggageUseCase>().ExecuteAsync(luggageId);
            SpectreHelper.MostrarExito($"Equipaje #{l.Id} enviado a bodega. Estado: {l.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }
}
