// src/UI/Admin/ClientesUsuarios/ClientMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;

namespace AirTicketSystem.UI.Admin.ClientesUsuarios;

public sealed class ClientMenu
{
    private readonly IServiceProvider _provider;

    public ClientMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Clientes");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todos",
                    "Listar activos",
                    "Ver cliente por ID",
                    "Ver reservas del cliente",
                    "Crear cliente",
                    "Agregar contacto de emergencia",
                    "Actualizar contacto de emergencia",
                    "Marcar contacto principal",
                    "Eliminar contacto de emergencia",
                    "Activar cliente",
                    "Desactivar cliente",
                    "Eliminar cliente",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todos":                    await ListarTodosAsync();               break;
                case "Listar activos":                  await ListarActivosAsync();             break;
                case "Ver cliente por ID":              await VerPorIdAsync();                  break;
                case "Ver reservas del cliente":        await VerReservasAsync();               break;
                case "Crear cliente":                   await CrearAsync();                     break;
                case "Agregar contacto de emergencia":  await AgregarContactoAsync();           break;
                case "Actualizar contacto de emergencia": await ActualizarContactoAsync();      break;
                case "Marcar contacto principal":       await MarcarPrincipalAsync();           break;
                case "Eliminar contacto de emergencia": await EliminarContactoAsync();          break;
                case "Activar cliente":                 await ActivarAsync();                   break;
                case "Desactivar cliente":              await DesactivarAsync();                break;
                case "Eliminar cliente":                await EliminarAsync();                  break;
                case "Volver":                          return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin clientes registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "UsuarioID", "Activo", "Registro", "Días como cliente");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla,
                    c.Id.ToString(), c.PersonaId.ToString(), c.UsuarioId.ToString(),
                    c.Activo.Valor ? "Sí" : "No",
                    c.FechaRegistro.Valor.ToString("yyyy-MM-dd"),
                    c.DiasComoCliente.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveClientsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin clientes activos."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "UsuarioID", "Registro");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.PersonaId.ToString(),
                    c.UsuarioId.ToString(), c.FechaRegistro.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del cliente");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<GetClientByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",              c.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "PersonaID",       c.PersonaId.ToString());
            SpectreHelper.AgregarFila(tabla, "UsuarioID",       c.UsuarioId.ToString());
            SpectreHelper.AgregarFila(tabla, "Activo",          c.Activo.Valor ? "Sí" : "No");
            SpectreHelper.AgregarFila(tabla, "Registro",        c.FechaRegistro.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Días cliente",    c.DiasComoCliente.ToString());
            SpectreHelper.AgregarFila(tabla, "Cliente nuevo",   c.EsClienteNuevo ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerReservasAsync()
    {
        var clienteId = SpectreHelper.PedirEntero("ID del cliente");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var reservas = await scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>().ExecuteAsync(clienteId);
            if (reservas.Count == 0) { SpectreHelper.MostrarInfo("Este cliente no tiene reservas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Código", "VueloID", "Estado", "Total", "Expiración");
            foreach (var r in reservas)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.CodigoReserva.Valor, r.VueloId.ToString(),
                    r.Estado.Valor, r.ValorTotal.Valor.ToString("C"),
                    r.FechaExpiracion?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Cliente");
        var personaId = SpectreHelper.PedirEntero("ID de la persona");
        var usuarioId = SpectreHelper.PedirEntero("ID del usuario");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CreateClientUseCase>().ExecuteAsync(personaId, usuarioId);
            SpectreHelper.MostrarExito($"Cliente creado (ID {c.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarContactoAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Contacto de Emergencia");
        SpectreHelper.MostrarInfo("El contacto debe ser una persona registrada en el sistema.");
        var clienteId      = SpectreHelper.PedirEntero("ID del cliente");
        var personaId      = SpectreHelper.PedirEntero("ID de la persona (contacto)");
        var relacionId     = SpectreHelper.PedirEntero("ID relación de contacto");
        var esPrincipalStr = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
        bool esPrincipal   = esPrincipalStr.Trim().ToLower() == "s";

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<AddEmergencyContactUseCase>()
                .ExecuteAsync(clienteId, personaId, relacionId, esPrincipal);
            SpectreHelper.MostrarExito($"Contacto de emergencia agregado (ID {c.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarContactoAsync()
    {
        var contactoId = SpectreHelper.PedirEntero("ID del contacto de emergencia");
        var relacionId = SpectreHelper.PedirEntero("Nuevo ID de relación de contacto");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<UpdateEmergencyContactUseCase>()
                .ExecuteAsync(contactoId, relacionId);
            SpectreHelper.MostrarExito("Contacto actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarPrincipalAsync()
    {
        var contactoId = SpectreHelper.PedirEntero("ID del contacto a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalEmergencyContactUseCase>().ExecuteAsync(contactoId);
            SpectreHelper.MostrarExito("Contacto marcado como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarContactoAsync()
    {
        var contactoId = SpectreHelper.PedirEntero("ID del contacto a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar el contacto?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteEmergencyContactUseCase>().ExecuteAsync(contactoId);
            SpectreHelper.MostrarExito("Contacto eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del cliente a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateClientUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Cliente activado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del cliente a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateClientUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Cliente desactivado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del cliente a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteClientUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Cliente eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
