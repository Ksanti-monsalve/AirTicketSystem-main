// src/UI/Admin/Aeronautica/ServiceClassMenuWrapper.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.serviceclass.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class ServiceClassMenuWrapper
{
    private readonly IServiceProvider _provider;

    public ServiceClassMenuWrapper(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Clases de Servicio");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar", "Crear", "Editar", "Eliminar", "Volver"]);

            if (opcion == "Volver") return;

            switch (opcion)
            {
                case "Listar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        var lista = await sc.ServiceProvider.GetRequiredService<GetAllServiceClassesUseCase>().ExecuteAsync();
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }
                        var t = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "Descripción");
                        foreach (var x in lista)
                            SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Nombre.Valor, x.Codigo.Valor, x.Descripcion?.Valor ?? "-");
                        SpectreHelper.MostrarTabla(t);
                        SpectreHelper.EsperarTecla();
                    });
                    break;

                case "Crear":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var nombre = SpectreHelper.PedirTexto("Nombre (ej: Ejecutiva)");
                        var codigo = SpectreHelper.PedirTexto("Código (ej: J)");
                        var desc   = SpectreHelper.PedirTexto("Descripción (opcional)");
                        string? descOpc = string.IsNullOrWhiteSpace(desc) ? null : desc;
                        await using var sc = _provider.CreateAsyncScope();
                        var r = await sc.ServiceProvider.GetRequiredService<CreateServiceClassUseCase>()
                            .ExecuteAsync(nombre, codigo, descOpc);
                        SpectreHelper.MostrarExito($"Clase '{r.Nombre.Valor}' creada (ID {r.Id}).");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Editar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id     = SpectreHelper.PedirEntero("ID de la clase");
                        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                        var desc   = SpectreHelper.PedirTexto("Nueva descripción (opcional)");
                        string? descOpc = string.IsNullOrWhiteSpace(desc) ? null : desc;
                        await using var sc = _provider.CreateAsyncScope();
                        var r = await sc.ServiceProvider.GetRequiredService<UpdateServiceClassUseCase>()
                            .ExecuteAsync(id, nombre, descOpc);
                        SpectreHelper.MostrarExito($"Clase '{r.Nombre.Valor}' actualizada.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Eliminar":
                    if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); break; }
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id = SpectreHelper.PedirEntero("ID a eliminar");
                        await using var sc = _provider.CreateAsyncScope();
                        await sc.ServiceProvider.GetRequiredService<DeleteServiceClassUseCase>().ExecuteAsync(id);
                        SpectreHelper.MostrarExito("Clase eliminada.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;
            }
        }
    }
}
