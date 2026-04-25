// src/UI/Admin/AirportsRoutes/FareMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.fare.Application.UseCases;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.serviceclass.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class FareMenu
{
    private readonly IServiceProvider _provider;

    public FareMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Tarifas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas",
                    "Listar activas",
                    "Listar por ruta",
                    "Listar activas por ruta",
                    "Crear",
                    "Editar",
                    "Activar",
                    "Desactivar",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":           await ListarTodasAsync();          break;
                case "Listar activas":         await ListarActivasAsync();        break;
                case "Listar por ruta":        await ListarPorRutaAsync();        break;
                case "Listar activas por ruta": await ListarActivasPorRutaAsync(); break;
                case "Crear":                  await CrearAsync();                break;
                case "Editar":                 await EditarAsync();               break;
                case "Activar":                await ActivarAsync();              break;
                case "Desactivar":             await DesactivarAsync();           break;
                case "Volver":                 return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllFaresUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin tarifas."); SpectreHelper.EsperarTecla(); return; }

            MostrarTablaFaras(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveFaresUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay tarifas activas."); SpectreHelper.EsperarTecla(); return; }

            MostrarTablaFaras(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorRutaAsync()
    {
        var rutaId = SpectreHelper.PedirEntero("ID de la ruta");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetFaresByRouteUseCase>()
                .ExecuteAsync(rutaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin tarifas para esa ruta."); SpectreHelper.EsperarTecla(); return; }

            MostrarTablaFaras(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasPorRutaAsync()
    {
        var rutaId = SpectreHelper.PedirEntero("ID de la ruta");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveFaresByRouteUseCase>()
                .ExecuteAsync(rutaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin tarifas activas para esa ruta."); SpectreHelper.EsperarTecla(); return; }

            MostrarTablaFaras(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Tarifa");
        await MostrarRutasAsync();
        var rutaId = SpectreHelper.PedirEntero("ID de la ruta");

        await MostrarClasesAsync();
        var claseId = SpectreHelper.PedirEntero("ID de la clase de servicio");

        var nombre          = SpectreHelper.PedirTexto("Nombre de la tarifa (ej: Económica Básica)");
        var precioBase      = SpectreHelper.PedirDecimal("Precio base");
        var impuestos       = SpectreHelper.PedirDecimal("Impuestos");
        var permiteCambios  = SpectreHelper.Confirmar("¿Permite cambios?");
        var permiteReembolso = SpectreHelper.Confirmar("¿Permite reembolso?");
        var vigStr          = SpectreHelper.PedirTexto("Vigente hasta (yyyy-MM-dd, opcional)");
        DateOnly? vigenteHasta = DateOnly.TryParse(vigStr, out var vd) ? vd : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var f = await scope.ServiceProvider.GetRequiredService<CreateFareUseCase>()
                .ExecuteAsync(rutaId, claseId, nombre, precioBase, impuestos,
                    permiteCambios, permiteReembolso, vigenteHasta);
            SpectreHelper.MostrarExito($"Tarifa '{f.Nombre.Valor}' creada (ID {f.Id}). Total: {f.PrecioTotal.Valor:C2}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Tarifa");
        var id              = SpectreHelper.PedirEntero("ID de la tarifa");
        var nombre          = SpectreHelper.PedirTexto("Nuevo nombre");
        var precioBase      = SpectreHelper.PedirDecimal("Nuevo precio base");
        var impuestos       = SpectreHelper.PedirDecimal("Nuevos impuestos");
        var permiteCambios  = SpectreHelper.Confirmar("¿Permite cambios?");
        var permiteReembolso = SpectreHelper.Confirmar("¿Permite reembolso?");
        var vigStr          = SpectreHelper.PedirTexto("Vigente hasta (yyyy-MM-dd, Enter para quitar vigencia)");
        DateOnly? vigenteHasta = DateOnly.TryParse(vigStr, out var vd) ? vd : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var f = await scope.ServiceProvider.GetRequiredService<UpdateFareUseCase>()
                .ExecuteAsync(id, nombre, precioBase, impuestos,
                    permiteCambios, permiteReembolso, vigenteHasta);
            SpectreHelper.MostrarExito($"Tarifa '{f.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la tarifa a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateFareUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Tarifa activada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la tarifa a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateFareUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Tarifa desactivada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private static void MostrarTablaFaras(IEnumerable<AirTicketSystem.modules.fare.Domain.aggregate.Fare> lista)
    {
        var tabla = SpectreHelper.CrearTabla("ID", "RutaID", "ClaseID", "Nombre", "Base", "Impuestos", "Total", "Cambios", "Reembolso", "Activa", "Vigente hasta");
        foreach (var f in lista)
            SpectreHelper.AgregarFila(tabla,
                f.Id.ToString(), f.RutaId.ToString(), f.ClaseServicioId.ToString(),
                f.Nombre.Valor,
                f.PrecioBase.Valor.ToString("C2"),
                f.Impuestos.Valor.ToString("C2"),
                f.PrecioTotal.Valor.ToString("C2"),
                f.PermiteCambios.Valor ? "Sí" : "No",
                f.PermiteReembolso.Valor ? "Sí" : "No",
                f.Activa.Valor ? "Sí" : "No",
                f.VigenteHasta?.Valor.ToString("yyyy-MM-dd") ?? "Sin vencimiento");
        SpectreHelper.MostrarTabla(tabla);
    }

    private async Task MostrarRutasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveRoutesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "AerolineaID", "OrigenID", "DestinoID");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.AerolineaId.ToString(),
                    r.OrigenId.ToString(), r.DestinoId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarClasesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllServiceClassesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Clase", "Código");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.Codigo.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
