// src/UI/Admin/Reservations/AdditionalChargeMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.additionalcharge.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class AdditionalChargeMenu
{
    private readonly IServiceProvider _provider;

    public AdditionalChargeMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Cargos Adicionales");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver cargos de reserva",
                    "Total cargos de reserva",
                    "Ver todos los cargos",
                    "Ver cargo por ID",
                    "Crear cargo adicional",
                    "Eliminar cargo",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver cargos de reserva":   await VerPorReservaAsync();  break;
                case "Total cargos de reserva": await TotalPorReservaAsync(); break;
                case "Ver todos los cargos":    await ListarTodosAsync();     break;
                case "Ver cargo por ID":        await VerPorIdAsync();        break;
                case "Crear cargo adicional":   await CrearAsync();           break;
                case "Eliminar cargo":          await EliminarAsync();        break;
                case "Volver":                  return;
            }
        }
    }

    private async Task VerPorReservaAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetChargesByBookingUseCase>().ExecuteAsync(reservaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin cargos adicionales para esta reserva."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Concepto", "Monto", "Fecha");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla,
                    c.Id.ToString(), c.Concepto.Valor,
                    c.Monto.Valor.ToString("C"),
                    c.Fecha.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task TotalPorReservaAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var total = await scope.ServiceProvider.GetRequiredService<GetTotalChargesByBookingUseCase>().ExecuteAsync(reservaId);
            SpectreHelper.MostrarInfo($"Total cargos adicionales de reserva #{reservaId}: {total:C}");
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAdditionalChargesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin cargos adicionales."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "ReservaID", "Concepto", "Monto", "Fecha");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla,
                    c.Id.ToString(), c.ReservaId.ToString(), c.Concepto.Valor,
                    c.Monto.Valor.ToString("C"), c.Fecha.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del cargo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<GetAdditionalChargeByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",        c.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "ReservaID", c.ReservaId.ToString());
            SpectreHelper.AgregarFila(tabla, "Concepto",  c.Concepto.Valor);
            SpectreHelper.AgregarFila(tabla, "Monto",     c.Monto.Valor.ToString("C"));
            SpectreHelper.AgregarFila(tabla, "Fecha",     c.Fecha.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Cargo Adicional");
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        var concepto  = SpectreHelper.PedirTexto("Concepto del cargo");
        var montoStr  = SpectreHelper.PedirTexto("Monto");
        if (!decimal.TryParse(montoStr, out var monto)) { SpectreHelper.MostrarError("Monto inválido."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CreateAdditionalChargeUseCase>()
                .ExecuteAsync(reservaId, concepto, monto);
            SpectreHelper.MostrarExito($"Cargo '{c.Concepto.Valor}' creado (ID {c.Id}). Monto: {c.Monto.Valor:C}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del cargo a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar el cargo?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteAdditionalChargeUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Cargo eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
