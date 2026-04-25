// src/UI/Admin/AirportsRoutes/AirlineMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airline.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class AirlineMenu
{
    private readonly IServiceProvider _provider;

    public AirlineMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Aerolíneas");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas", "Listar activas", "Crear", "Editar",
                    "Activar", "Desactivar",
                    "Agregar email", "Eliminar email",
                    "Agregar teléfono", "Eliminar teléfono",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":       await ListarTodasAsync();      break;
                case "Listar activas":     await ListarActivasAsync();    break;
                case "Crear":              await CrearAsync();            break;
                case "Editar":             await EditarAsync();           break;
                case "Activar":            await ActivarAsync();          break;
                case "Desactivar":         await DesactivarAsync();       break;
                case "Agregar email":      await AgregarEmailAsync();     break;
                case "Eliminar email":     await EliminarEmailAsync();    break;
                case "Agregar teléfono":   await AgregarTelefonoAsync();  break;
                case "Eliminar teléfono":  await EliminarTelefonoAsync(); break;
                case "Volver":             return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAirlinesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin aerolíneas."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Comercial", "IATA", "ICAO", "Activa");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.NombreComercial?.Valor ?? "-", a.CodigoIata.Valor, a.CodigoIcao.Valor,
                    a.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirlinesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay aerolíneas activas."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA", "ICAO");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor, a.CodigoIata.Valor, a.CodigoIcao.Valor);
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Aerolínea");
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);
        if (pais is null) return;

        var iata  = SpectreHelper.PedirTexto("Código IATA — 2 letras (ej: AV)");
        var icao  = SpectreHelper.PedirTexto("Código ICAO — 3 letras (ej: AVA)");
        var nombre= SpectreHelper.PedirTexto("Nombre oficial");
        var ncOpc = SpectreHelper.PedirTexto("Nombre comercial (opcional)", obligatorio: false);
        var webOpc= SpectreHelper.PedirTexto("Sitio web (opcional)", obligatorio: false);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateAirlineUseCase>()
                .ExecuteAsync(pais.Id, iata, icao, nombre,
                    string.IsNullOrWhiteSpace(ncOpc) ? null : ncOpc,
                    string.IsNullOrWhiteSpace(webOpc) ? null : webOpc);
            SpectreHelper.MostrarExito($"Aerolínea '{r.Nombre.Valor}' [{r.CodigoIata.Valor}] creada (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var aerolinea = await SelectorUI.SeleccionarAerolineaAsync(_provider);
        if (aerolinea is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {aerolinea.Nombre.Valor} [{aerolinea.CodigoIata.Valor}]");
        var nombre= SpectreHelper.PedirTexto($"Nuevo nombre oficial  (actual: {aerolinea.Nombre.Valor})");
        var ncOpc = SpectreHelper.PedirTexto($"Nuevo nombre comercial  (actual: {aerolinea.NombreComercial?.Valor ?? "-"})", obligatorio: false);
        var webOpc= SpectreHelper.PedirTexto($"Nuevo sitio web  (actual: {aerolinea.SitioWeb?.Valor ?? "-"})", obligatorio: false);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateAirlineUseCase>()
                .ExecuteAsync(aerolinea.Id, nombre,
                    string.IsNullOrWhiteSpace(ncOpc) ? null : ncOpc,
                    string.IsNullOrWhiteSpace(webOpc) ? null : webOpc);
            SpectreHelper.MostrarExito($"Aerolínea '{r.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var aerolinea = await SelectorUI.SeleccionarAerolineaAsync(_provider);
        if (aerolinea is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateAirlineUseCase>().ExecuteAsync(aerolinea.Id);
            SpectreHelper.MostrarExito($"Aerolínea '{aerolinea.Nombre.Valor}' activada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var aerolinea = await SelectorUI.SeleccionarAerolineaAsync(_provider);
        if (aerolinea is null) return;
        if (!SpectreHelper.Confirmar($"¿Desactivar '{aerolinea.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateAirlineUseCase>().ExecuteAsync(aerolinea.Id);
            SpectreHelper.MostrarExito($"Aerolínea '{aerolinea.Nombre.Valor}' desactivada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarEmailAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Email a Aerolínea");
        var aerolinea   = await SelectorUI.SeleccionarAerolineaAsync(_provider);
        if (aerolinea is null) return;
        var tipoEmail   = await SelectorUI.SeleccionarTipoEmailAsync(_provider);
        if (tipoEmail is null) return;
        var email       = SpectreHelper.PedirTexto("Dirección de email");
        var esPrincipal = SpectreHelper.Confirmar("¿Es el email principal?");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<AddAirlineEmailUseCase>()
                .ExecuteAsync(aerolinea.Id, tipoEmail.Id, email, esPrincipal);
            SpectreHelper.MostrarExito($"Email '{r.Email.Valor}' agregado a {aerolinea.Nombre.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarEmailAsync()
    {
        var emailId = SpectreHelper.PedirEntero("ID del email a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RemoveAirlineEmailUseCase>().ExecuteAsync(emailId);
            SpectreHelper.MostrarExito("Email eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarTelefonoAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Teléfono a Aerolínea");
        var aerolinea  = await SelectorUI.SeleccionarAerolineaAsync(_provider);
        if (aerolinea is null) return;
        var tipoTel    = await SelectorUI.SeleccionarTipoTelefonoAsync(_provider);
        if (tipoTel is null) return;
        var numero     = SpectreHelper.PedirTexto("Número de teléfono");
        var indicativo = SpectreHelper.PedirTexto("Indicativo país (ej: +57, opcional)", obligatorio: false);
        var esPrincipal= SpectreHelper.Confirmar("¿Es el teléfono principal?");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<AddAirlinePhoneUseCase>()
                .ExecuteAsync(aerolinea.Id, tipoTel.Id, numero,
                    string.IsNullOrWhiteSpace(indicativo) ? null : indicativo, esPrincipal);
            SpectreHelper.MostrarExito($"Teléfono '{r.Numero.Valor}' agregado a {aerolinea.Nombre.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarTelefonoAsync()
    {
        var telefonoId = SpectreHelper.PedirEntero("ID del teléfono a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RemoveAirlinePhoneUseCase>().ExecuteAsync(telefonoId);
            SpectreHelper.MostrarExito("Teléfono eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
