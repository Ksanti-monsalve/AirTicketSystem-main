// src/UI/Client/MyProfileMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.user.Application.UseCases;
using AirTicketSystem.modules.person.Application.UseCases;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.UI.Client;

public sealed class MyProfileMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public MyProfileMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mi Perfil");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "3.1 Ver mis datos personales",
                    "3.2 Modificar mis datos",
                    "3.3 Gestionar mis teléfonos",
                    "3.4 Gestionar mis emails",
                    "3.5 Gestionar mis direcciones",
                    "3.6 Gestionar contactos de emergencia",
                    "Cambiar contraseña",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "3.1 Ver mis datos personales":      await VerInfoAsync();              break;
                case "3.2 Modificar mis datos":           await ModificarDatosAsync();       break;
                case "3.3 Gestionar mis teléfonos":       await GestionarTelefonosAsync();   break;
                case "3.4 Gestionar mis emails":          await GestionarEmailsAsync();      break;
                case "3.5 Gestionar mis direcciones":     await GestionarDireccionesAsync(); break;
                case "3.6 Gestionar contactos de emergencia": await GestionarContactosAsync(); break;
                case "Cambiar contraseña":                await CambiarPasswordAsync();      break;
                case "Volver":                            return;
            }
        }
    }

    // ── 3.1 Ver mis datos ────────────────────────────────────────────────────
    private async Task VerInfoAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var u = await scope.ServiceProvider.GetRequiredService<GetUserByIdUseCase>()
                .ExecuteAsync(_session.CurrentUserId);

            // Buscar cliente y persona asociados
            var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            var cliente  = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "Username",      u.Username.Valor);
            SpectreHelper.AgregarFila(tabla, "Activo",        u.Activo.Valor ? "Sí" : "No");
            SpectreHelper.AgregarFila(tabla, "Registro",      u.FechaRegistro.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Último acceso", u.UltimoLogin?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-");

            if (cliente != null)
            {
                SpectreHelper.AgregarFila(tabla, "ClienteID",  cliente.Id.ToString());
                SpectreHelper.AgregarFila(tabla, "PersonaID",  cliente.PersonaId.ToString());
                SpectreHelper.AgregarFila(tabla, "Días cliente", cliente.DiasComoCliente.ToString());

                var persona = await scope.ServiceProvider.GetRequiredService<GetPersonByIdUseCase>().ExecuteAsync(cliente.PersonaId);
                SpectreHelper.AgregarFila(tabla, "Nombres",   persona.Nombres.Valor);
                SpectreHelper.AgregarFila(tabla, "Apellidos", persona.Apellidos.Valor);
                SpectreHelper.AgregarFila(tabla, "Documento", persona.NumeroDoc.Valor);
                SpectreHelper.AgregarFila(tabla, "Fecha nac.", persona.FechaNacimiento?.Valor.ToString("yyyy-MM-dd") ?? "-");
            }

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 3.2 Modificar mis datos ──────────────────────────────────────────────
    private async Task ModificarDatosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            var cliente  = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId);
            if (cliente is null) { SpectreHelper.MostrarError("No se encontró el perfil de cliente."); SpectreHelper.EsperarTecla(); return; }

            SpectreHelper.MostrarSubtitulo("Modificar Mis Datos");
            var nombres   = SpectreHelper.PedirTexto("Nuevos nombres");
            var apellidos = SpectreHelper.PedirTexto("Nuevos apellidos");
            var fechaStr  = SpectreHelper.PedirTexto("Nueva fecha de nacimiento (yyyy-MM-dd, opcional)");
            DateOnly? fecha = DateOnly.TryParse(fechaStr, out var f) ? f : null;

            await scope.ServiceProvider.GetRequiredService<UpdatePersonUseCase>().ExecuteAsync(cliente.PersonaId, nombres, apellidos, fecha, null, null, default);
            SpectreHelper.MostrarExito("Datos actualizados correctamente.");
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 3.3 Gestionar teléfonos ──────────────────────────────────────────────
    private async Task GestionarTelefonosAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mis Teléfonos");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Ver mis teléfonos", "Agregar teléfono", "Eliminar teléfono", "Marcar principal", "Volver"]);
            if (opcion == "Volver") return;

            await ConsoleErrorHandler.ExecuteAsync(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
                var cliente  = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId);
                if (cliente is null) { SpectreHelper.MostrarError("Perfil no encontrado."); SpectreHelper.EsperarTecla(); return; }

                switch (opcion)
                {
                    case "Ver mis teléfonos":
                        var repo  = scope.ServiceProvider.GetRequiredService<IPersonPhoneRepository>();
                        var lista = await repo.FindByPersonaAsync(cliente.PersonaId);
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin teléfonos registrados."); SpectreHelper.EsperarTecla(); return; }
                        var tabla = SpectreHelper.CrearTabla("ID", "Número", "Tipo", "Principal");
                        foreach (var ph in lista)
                            SpectreHelper.AgregarFila(tabla, ph.Id.ToString(), ph.Numero.Valor,
                                ph.TipoTelefonoId.ToString(), ph.EsPrincipal.Valor ? "Sí" : "No");
                        SpectreHelper.MostrarTabla(tabla);
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Agregar teléfono":
                        var tipoTel2  = await SelectorUI.SeleccionarTipoTelefonoAsync(_provider);
                        if (tipoTel2 is null) { SpectreHelper.EsperarTecla(); return; }
                        var numero    = SpectreHelper.PedirTexto("Número");
                        var indic     = SpectreHelper.PedirTexto("Indicativo país (ej: +57, opcional)", obligatorio: false);
                        var prinStr   = SpectreHelper.PedirTexto("¿Principal? (s/n)");
                        string? indOpc = string.IsNullOrWhiteSpace(indic) ? null : indic;
                        var ph2 = await scope.ServiceProvider.GetRequiredService<AddPersonPhoneUseCase>()
                            .ExecuteAsync(cliente.PersonaId, tipoTel2.Id, numero, indOpc, prinStr.Trim().ToLower() == "s");
                        SpectreHelper.MostrarExito($"Teléfono '{ph2.Numero.Valor}' agregado.");
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Eliminar teléfono":
                        var idEl = SpectreHelper.PedirEntero("ID del teléfono");
                        if (SpectreHelper.Confirmar("¿Confirma eliminar?"))
                        {
                            await scope.ServiceProvider.GetRequiredService<DeletePersonPhoneUseCase>().ExecuteAsync(idEl);
                            SpectreHelper.MostrarExito("Teléfono eliminado.");
                        }
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Marcar principal":
                        var idPrin = SpectreHelper.PedirEntero("ID del teléfono a marcar como principal");
                        await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonPhoneUseCase>().ExecuteAsync(idPrin);
                        SpectreHelper.MostrarExito("Teléfono marcado como principal.");
                        SpectreHelper.EsperarTecla();
                        break;
                }
            });
        }
    }

    // ── 3.4 Gestionar emails ─────────────────────────────────────────────────
    private async Task GestionarEmailsAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mis Emails");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Ver mis emails", "Agregar email", "Eliminar email", "Marcar principal", "Volver"]);
            if (opcion == "Volver") return;

            await ConsoleErrorHandler.ExecuteAsync(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
                var cliente  = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId);
                if (cliente is null) { SpectreHelper.MostrarError("Perfil no encontrado."); SpectreHelper.EsperarTecla(); return; }

                switch (opcion)
                {
                    case "Ver mis emails":
                        var repo  = scope.ServiceProvider.GetRequiredService<IPersonEmailRepository>();
                        var lista = await repo.FindByPersonaAsync(cliente.PersonaId);
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin emails registrados."); SpectreHelper.EsperarTecla(); return; }
                        var tabla = SpectreHelper.CrearTabla("ID", "Email", "Tipo", "Principal");
                        foreach (var em in lista)
                            SpectreHelper.AgregarFila(tabla, em.Id.ToString(), em.Email.Valor,
                                em.TipoEmailId.ToString(), em.EsPrincipal.Valor ? "Sí" : "No");
                        SpectreHelper.MostrarTabla(tabla);
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Agregar email":
                        var tipoEm2  = await SelectorUI.SeleccionarTipoEmailAsync(_provider);
                        if (tipoEm2 is null) { SpectreHelper.EsperarTecla(); return; }
                        var email    = SpectreHelper.PedirTexto("Dirección de email");
                        var prinStr  = SpectreHelper.PedirTexto("¿Principal? (s/n)");
                        var em2 = await scope.ServiceProvider.GetRequiredService<AddPersonEmailUseCase>().ExecuteAsync(cliente.PersonaId, tipoEm2.Id, email, prinStr.Trim().ToLower() == "s", default);
                        SpectreHelper.MostrarExito($"Email '{em2.Email.Valor}' agregado.");
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Eliminar email":
                        var idEl = SpectreHelper.PedirEntero("ID del email");
                        if (SpectreHelper.Confirmar("¿Confirma eliminar?"))
                        {
                            await scope.ServiceProvider.GetRequiredService<DeletePersonEmailUseCase>().ExecuteAsync(idEl);
                            SpectreHelper.MostrarExito("Email eliminado.");
                        }
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Marcar principal":
                        var idPrin = SpectreHelper.PedirEntero("ID del email a marcar como principal");
                        await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonEmailUseCase>().ExecuteAsync(idPrin);
                        SpectreHelper.MostrarExito("Email marcado como principal.");
                        SpectreHelper.EsperarTecla();
                        break;
                }
            });
        }
    }

    // ── 3.5 Gestionar direcciones ────────────────────────────────────────────
    private async Task GestionarDireccionesAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mis Direcciones");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Ver mis direcciones", "Agregar dirección", "Eliminar dirección", "Marcar principal", "Volver"]);
            if (opcion == "Volver") return;

            await ConsoleErrorHandler.ExecuteAsync(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
                var cliente  = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId);
                if (cliente is null) { SpectreHelper.MostrarError("Perfil no encontrado."); SpectreHelper.EsperarTecla(); return; }

                switch (opcion)
                {
                    case "Ver mis direcciones":
                        var repo  = scope.ServiceProvider.GetRequiredService<IPersonAddressRepository>();
                        var lista = await repo.FindByPersonaAsync(cliente.PersonaId);
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin direcciones registradas."); SpectreHelper.EsperarTecla(); return; }
                        var tabla = SpectreHelper.CrearTabla("ID", "Dirección", "CiudadID", "Principal");
                        foreach (var addr in lista)
                            SpectreHelper.AgregarFila(tabla, addr.Id.ToString(), addr.DireccionLinea1.Valor,
                                addr.CiudadId.ToString(), addr.EsPrincipal.Valor ? "Sí" : "No");
                        SpectreHelper.MostrarTabla(tabla);
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Agregar dirección":
                        var tipoDir2 = await SelectorUI.SeleccionarTipoDireccionAsync(_provider);
                        if (tipoDir2 is null) { SpectreHelper.EsperarTecla(); return; }
                        var ciudad2  = await SelectorUI.SeleccionarCiudadAsync(_provider);
                        if (ciudad2 is null) { SpectreHelper.EsperarTecla(); return; }
                        var dir      = SpectreHelper.PedirTexto("Dirección completa");
                        var prinStr  = SpectreHelper.PedirTexto("¿Principal? (s/n)");
                        var addr2 = await scope.ServiceProvider.GetRequiredService<AddPersonAddressUseCase>()
                            .ExecuteAsync(cliente.PersonaId, tipoDir2.Id, ciudad2.Id, dir, null, null, prinStr.Trim().ToLower() == "s");
                        SpectreHelper.MostrarExito($"Dirección agregada (ID {addr2.Id}).");
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Eliminar dirección":
                        var idEl = SpectreHelper.PedirEntero("ID de la dirección");
                        if (SpectreHelper.Confirmar("¿Confirma eliminar?"))
                        {
                            await scope.ServiceProvider.GetRequiredService<DeletePersonAddressUseCase>().ExecuteAsync(idEl);
                            SpectreHelper.MostrarExito("Dirección eliminada.");
                        }
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Marcar principal":
                        var idPrin = SpectreHelper.PedirEntero("ID de la dirección a marcar como principal");
                        await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonAddressUseCase>().ExecuteAsync(idPrin);
                        SpectreHelper.MostrarExito("Dirección marcada como principal.");
                        SpectreHelper.EsperarTecla();
                        break;
                }
            });
        }
    }

    // ── 3.6 Gestionar contactos de emergencia ────────────────────────────────
    private async Task GestionarContactosAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mis Contactos de Emergencia");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Agregar contacto", "Actualizar contacto", "Marcar principal", "Eliminar contacto", "Volver"]);
            if (opcion == "Volver") return;

            await ConsoleErrorHandler.ExecuteAsync(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
                var cliente  = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId);
                if (cliente is null) { SpectreHelper.MostrarError("Perfil no encontrado."); SpectreHelper.EsperarTecla(); return; }

                switch (opcion)
                {
                    case "Agregar contacto":
                        SpectreHelper.MostrarInfo("El contacto debe ser una persona registrada en el sistema.");
                        var personaIdC = SpectreHelper.PedirEntero("ID de la persona (contacto)");
                        var relacionId = SpectreHelper.PedirEntero("ID de la relación de contacto");
                        var prinStr    = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
                        var c2 = await scope.ServiceProvider.GetRequiredService<AddEmergencyContactUseCase>()
                            .ExecuteAsync(cliente.Id, personaIdC, relacionId, prinStr.Trim().ToLower() == "s");
                        SpectreHelper.MostrarExito($"Contacto de emergencia agregado correctamente (ID {c2.Id}).");
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Actualizar contacto":
                        var idUpd  = SpectreHelper.PedirEntero("ID del contacto");
                        var relUpd = SpectreHelper.PedirEntero("Nuevo ID de relación de contacto");
                        await scope.ServiceProvider.GetRequiredService<UpdateEmergencyContactUseCase>().ExecuteAsync(idUpd, relUpd);
                        SpectreHelper.MostrarExito("Contacto actualizado.");
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Marcar principal":
                        var idPrin = SpectreHelper.PedirEntero("ID del contacto a marcar principal");
                        await scope.ServiceProvider.GetRequiredService<SetPrincipalEmergencyContactUseCase>().ExecuteAsync(idPrin);
                        SpectreHelper.MostrarExito("Contacto marcado como principal.");
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Eliminar contacto":
                        var idEl = SpectreHelper.PedirEntero("ID del contacto a eliminar");
                        if (SpectreHelper.Confirmar("¿Confirma eliminar?"))
                        {
                            await scope.ServiceProvider.GetRequiredService<DeleteEmergencyContactUseCase>().ExecuteAsync(idEl);
                            SpectreHelper.MostrarExito("Contacto eliminado.");
                        }
                        SpectreHelper.EsperarTecla();
                        break;
                }
            });
        }
    }

    // ── Cambiar contraseña ───────────────────────────────────────────────────
    private async Task CambiarPasswordAsync()
    {
        var actual   = SpectreHelper.PedirTexto("Contraseña actual");
        var nueva    = SpectreHelper.PedirTexto("Nueva contraseña");
        var confirma = SpectreHelper.PedirTexto("Confirme la nueva contraseña");

        if (nueva != confirma)
        {
            SpectreHelper.MostrarError("Las contraseñas no coinciden.");
            SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeV = _provider.CreateAsyncScope();
            var u = await scopeV.ServiceProvider.GetRequiredService<GetUserByIdUseCase>()
                .ExecuteAsync(_session.CurrentUserId);

            if (!PasswordHasher.Verify(actual, u.PasswordHash.Valor))
                throw new InvalidOperationException("La contraseña actual es incorrecta.");

            var nuevoHash = PasswordHasher.Hash(nueva);
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ChangePasswordUseCase>()
                .ExecuteAsync(_session.CurrentUserId, nuevoHash);
            SpectreHelper.MostrarExito("Contraseña actualizada correctamente.");
        });
        SpectreHelper.EsperarTecla();
    }
}
