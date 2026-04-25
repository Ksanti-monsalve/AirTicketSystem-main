// src/UI/Admin/ClientesUsuarios/PersonasMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.person.Application.UseCases;

namespace AirTicketSystem.UI.Admin.ClientesUsuarios;

public sealed class PersonasMenu
{
    private readonly IServiceProvider _provider;

    public PersonasMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Personas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas",
                    "Buscar por ID",
                    "Buscar por documento",
                    "Crear persona",
                    "Actualizar persona",
                    "Agregar teléfono",
                    "Eliminar teléfono",
                    "Marcar teléfono principal",
                    "Agregar email",
                    "Eliminar email",
                    "Marcar email principal",
                    "Agregar dirección",
                    "Eliminar dirección",
                    "Marcar dirección principal",
                    "Eliminar persona",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":              await ListarTodasAsync();           break;
                case "Buscar por ID":             await BuscarPorIdAsync();           break;
                case "Buscar por documento":      await BuscarPorDocumentoAsync();    break;
                case "Crear persona":             await CrearAsync();                 break;
                case "Actualizar persona":        await ActualizarAsync();            break;
                case "Agregar teléfono":          await AgregarTelefonoAsync();       break;
                case "Eliminar teléfono":         await EliminarTelefonoAsync();      break;
                case "Marcar teléfono principal": await MarcarTelefonoPrincipalAsync(); break;
                case "Agregar email":             await AgregarEmailAsync();          break;
                case "Eliminar email":            await EliminarEmailAsync();         break;
                case "Marcar email principal":    await MarcarEmailPrincipalAsync();  break;
                case "Agregar dirección":         await AgregarDireccionAsync();      break;
                case "Eliminar dirección":        await EliminarDireccionAsync();     break;
                case "Marcar dirección principal": await MarcarDireccionPrincipalAsync(); break;
                case "Eliminar persona":          await EliminarAsync();              break;
                case "Volver":                    return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllPersonsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin personas registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "TipoDocID", "Número Doc", "Nombres", "Apellidos");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla,
                    p.Id.ToString(), p.TipoDocId.ToString(), p.NumeroDoc.Valor,
                    p.Nombres.Valor, p.Apellidos.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la persona");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<GetPersonByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",              p.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "TipoDocID",       p.TipoDocId.ToString());
            SpectreHelper.AgregarFila(tabla, "Número Doc",      p.NumeroDoc.Valor);
            SpectreHelper.AgregarFila(tabla, "Nombres",         p.Nombres.Valor);
            SpectreHelper.AgregarFila(tabla, "Apellidos",       p.Apellidos.Valor);
            SpectreHelper.AgregarFila(tabla, "Fecha nac.",      p.FechaNacimiento?.Valor.ToString("yyyy-MM-dd") ?? "-");
            SpectreHelper.AgregarFila(tabla, "GeneroID",        p.GeneroId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "NacionalidadID",  p.NacionalidadId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Menor de edad",   p.EsMenorDeEdad ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarPorDocumentoAsync()
    {
        var tipoDoc = await SelectorUI.SeleccionarTipoDocumentoAsync(_provider);
        if (tipoDoc is null) return;
        var numero  = SpectreHelper.PedirTexto("Número de documento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<GetPersonByDocumentUseCase>().ExecuteAsync(tipoDoc.Id, numero);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",       p.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Nombres",  p.Nombres.Valor);
            SpectreHelper.AgregarFila(tabla, "Apellidos", p.Apellidos.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Persona");
        var tipoDoc = await SelectorUI.SeleccionarTipoDocumentoAsync(_provider);
        if (tipoDoc is null) return;

        var numeroDoc = SpectreHelper.PedirTexto("Número de documento");
        var nombres   = SpectreHelper.PedirTexto("Nombres");
        var apellidos = SpectreHelper.PedirTexto("Apellidos");
        var fechaStr  = SpectreHelper.PedirTexto("Fecha de nacimiento (yyyy-MM-dd, opcional)", obligatorio: false);

        SpectreHelper.MostrarSubtitulo("Género (opcional)");
        var genero = await SelectorUI.SeleccionarGeneroAsync(_provider);

        SpectreHelper.MostrarSubtitulo("Nacionalidad (opcional)");
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);

        DateOnly? fecha = DateOnly.TryParse(fechaStr, out var f) ? f : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<CreatePersonUseCase>()
                .ExecuteAsync(tipoDoc.Id, numeroDoc, nombres, apellidos, fecha, genero?.Id, pais?.Id);
            SpectreHelper.MostrarExito($"Persona '{p.NombreCompleto}' creada (ID {p.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarAsync()
    {
        // Seleccionar persona de la lista → garantiza existencia
        var persona = await SelectorUI.SeleccionarPersonaAsync(_provider);
        if (persona is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {persona.NombreCompleto}");
        var nombres   = SpectreHelper.PedirTexto($"Nuevos nombres  (actual: {persona.Nombres.Valor})");
        var apellidos = SpectreHelper.PedirTexto($"Nuevos apellidos  (actual: {persona.Apellidos.Valor})");
        var fechaStr  = SpectreHelper.PedirTexto($"Nueva fecha nac. (yyyy-MM-dd, actual: {persona.FechaNacimiento?.Valor.ToString("yyyy-MM-dd") ?? "-"})", obligatorio: false);

        SpectreHelper.MostrarSubtitulo("Nuevo género (opcional — Enter para omitir)");
        var genero = await SelectorUI.SeleccionarGeneroAsync(_provider);

        DateOnly? fecha = DateOnly.TryParse(fechaStr, out var f) ? f : persona.FechaNacimiento?.Valor;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<UpdatePersonUseCase>()
                .ExecuteAsync(persona.Id, nombres, apellidos, fecha, genero?.Id, null);
            SpectreHelper.MostrarExito("Persona actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarTelefonoAsync()
    {
        var persona    = await SelectorUI.SeleccionarPersonaAsync(_provider);
        if (persona is null) return;
        var tipoTel    = await SelectorUI.SeleccionarTipoTelefonoAsync(_provider);
        if (tipoTel is null) return;
        var numero     = SpectreHelper.PedirTexto("Número de teléfono");
        var indicativo = SpectreHelper.PedirTexto("Indicativo país (ej: +57, opcional)", obligatorio: false);
        var principalStr = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
        bool esPrincipal = principalStr.Trim().ToLower() == "s";
        string? indOpc   = string.IsNullOrWhiteSpace(indicativo) ? null : indicativo;
        // alias para compatibilidad
        var personaId = persona.Id;
        var tipoTelId = tipoTel.Id;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var ph = await scope.ServiceProvider.GetRequiredService<AddPersonPhoneUseCase>()
                .ExecuteAsync(personaId, tipoTelId, numero, indOpc, esPrincipal);
            SpectreHelper.MostrarExito($"Teléfono '{ph.Numero.Valor}' agregado (ID {ph.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarTelefonoAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del teléfono a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonPhoneUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Teléfono eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarTelefonoPrincipalAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del teléfono a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonPhoneUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Teléfono marcado como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarEmailAsync()
    {
        var persona    = await SelectorUI.SeleccionarPersonaAsync(_provider);
        if (persona is null) return;
        var tipoEmail  = await SelectorUI.SeleccionarTipoEmailAsync(_provider);
        if (tipoEmail is null) return;
        var email      = SpectreHelper.PedirTexto("Dirección de email");
        var prinStr    = SpectreHelper.PedirTexto("¿Es principal? (s/n)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var em = await scope.ServiceProvider.GetRequiredService<AddPersonEmailUseCase>()
                .ExecuteAsync(persona.Id, tipoEmail.Id, email, prinStr.Trim().ToLower() == "s");
            SpectreHelper.MostrarExito($"Email '{em.Email.Valor}' agregado a {persona.NombreCompleto}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarEmailAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del email a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonEmailUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Email eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarEmailPrincipalAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del email a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonEmailUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Email marcado como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarDireccionAsync()
    {
        var persona    = await SelectorUI.SeleccionarPersonaAsync(_provider);
        if (persona is null) return;
        var tipoDir    = await SelectorUI.SeleccionarTipoDireccionAsync(_provider);
        if (tipoDir is null) return;
        var ciudad     = await SelectorUI.SeleccionarCiudadAsync(_provider);
        if (ciudad is null) return;
        // alias para compatibilidad con el código existente
        var personaId = persona.Id;
        var tipoDirId = tipoDir.Id;
        var direccion    = SpectreHelper.PedirTexto("Dirección completa");
        var principalStr = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
        bool esPrincipal = principalStr.Trim().ToLower() == "s";

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var addr = await scope.ServiceProvider.GetRequiredService<AddPersonAddressUseCase>()
                .ExecuteAsync(personaId, tipoDirId, ciudad.Id, direccion, null, null, esPrincipal);
            SpectreHelper.MostrarExito($"Dirección agregada (ID {addr.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarDireccionAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la dirección a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonAddressUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Dirección eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarDireccionPrincipalAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la dirección a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonAddressUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Dirección marcada como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la persona a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Persona eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
