// src/UI/Admin/GeoConfig/ContinentMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.continent.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class ContinentMenu
{
    private readonly IServiceProvider _provider;

    public ContinentMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Continentes");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar":   await ListarAsync();   break;
                case "Crear":    await CrearAsync();    break;
                case "Editar":   await EditarAsync();   break;
                case "Eliminar": await EliminarAsync(); break;
                case "Volver":   return;
            }
        }
    }

    private async Task ListarAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllContinentsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay continentes registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.Codigo.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Continente");
        var nombre = SpectreHelper.PedirTexto("Nombre del continente");
        var codigo = SpectreHelper.PedirTexto("Código de 2 letras (ej: AM, EU, AS)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var result = await scope.ServiceProvider.GetRequiredService<CreateContinentUseCase>()
                .ExecuteAsync(nombre, codigo);
            SpectreHelper.MostrarExito($"Continente '{result.Nombre.Valor}' creado (ID {result.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        // Seleccionar desde lista → garantiza que el registro existe
        var continente = await SelectorUI.SeleccionarContinenteAsync(_provider);
        if (continente is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {continente.Nombre.Valor} [{continente.Codigo.Valor}]");
        var nombre = SpectreHelper.PedirTexto($"Nuevo nombre (actual: {continente.Nombre.Valor})");
        var codigo = SpectreHelper.PedirTexto($"Nuevo código (actual: {continente.Codigo.Valor})");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var result = await scope.ServiceProvider.GetRequiredService<UpdateContinentUseCase>()
                .ExecuteAsync(continente.Id, nombre, codigo);
            SpectreHelper.MostrarExito($"Continente '{result.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var continente = await SelectorUI.SeleccionarContinenteAsync(_provider);
        if (continente is null) return;

        if (!SpectreHelper.Confirmar($"¿Eliminar '{continente.Nombre.Valor}'?"))
        {
            SpectreHelper.MostrarInfo("Operación cancelada.");
            SpectreHelper.EsperarTecla();
            return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteContinentUseCase>()
                .ExecuteAsync(continente.Id);
            SpectreHelper.MostrarExito($"Continente '{continente.Nombre.Valor}' eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
