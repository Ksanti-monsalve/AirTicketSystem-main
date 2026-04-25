// src/UI/Admin/Billing/UserAdminMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.user.Application.UseCases;
using AirTicketSystem.modules.role.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Billing;

public sealed class UserAdminMenu
{
    private readonly IServiceProvider _provider;

    public UserAdminMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Administración de Usuarios");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todos los usuarios",
                    "Buscar por username",
                    "Crear usuario",
                    "Cambiar contraseña",
                    "Cambiar rol",
                    "Activar usuario",
                    "Desactivar usuario",
                    "Listar roles",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todos los usuarios": await ListarTodosAsync();       break;
                case "Buscar por username":       await BuscarPorUsernameAsync(); break;
                case "Crear usuario":             await CrearAsync();             break;
                case "Cambiar contraseña":        await CambiarPasswordAsync();   break;
                case "Cambiar rol":               await CambiarRolAsync();        break;
                case "Activar usuario":           await ActivarAsync();           break;
                case "Desactivar usuario":        await DesactivarAsync();        break;
                case "Listar roles":              await ListarRolesAsync();       break;
                case "Volver":                    return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllUsersUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin usuarios registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Username", "PersonaID", "RolID", "Activo", "Registro", "Último login");
            foreach (var u in lista)
                SpectreHelper.AgregarFila(tabla,
                    u.Id.ToString(), u.Username.Valor,
                    u.PersonaId.ToString(), u.RolId.ToString(),
                    u.Activo.Valor ? "Sí" : "No",
                    u.FechaRegistro.Valor.ToString("yyyy-MM-dd"),
                    u.UltimoLogin?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarPorUsernameAsync()
    {
        var username = SpectreHelper.PedirTexto("Nombre de usuario");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var u = await scope.ServiceProvider.GetRequiredService<GetUserByUsernameUseCase>()
                .ExecuteAsync(username);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",           u.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Username",     u.Username.Valor);
            SpectreHelper.AgregarFila(tabla, "PersonaID",    u.PersonaId.ToString());
            SpectreHelper.AgregarFila(tabla, "RolID",        u.RolId.ToString());
            SpectreHelper.AgregarFila(tabla, "Activo",       u.Activo.Valor ? "Sí" : "No");
            SpectreHelper.AgregarFila(tabla, "Registro",     u.FechaRegistro.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Último login", u.UltimoLogin?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-");
            SpectreHelper.AgregarFila(tabla, "Intentos fallidos", u.IntentosFallidos.Valor.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Usuario");
        await ListarRolesAsync();
        var personaId = SpectreHelper.PedirEntero("ID de la persona");
        var rolId     = SpectreHelper.PedirEntero("ID del rol");
        var username  = SpectreHelper.PedirTexto("Nombre de usuario");
        var password  = SpectreHelper.PedirTexto("Contraseña");
        var hash      = AirTicketSystem.shared.helpers.PasswordHasher.Hash(password);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var u = await scope.ServiceProvider.GetRequiredService<CreateUserUseCase>()
                .ExecuteAsync(personaId, rolId, username, hash);
            SpectreHelper.MostrarExito($"Usuario '{u.Username.Valor}' creado (ID {u.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CambiarPasswordAsync()
    {
        var id           = SpectreHelper.PedirEntero("ID del usuario");
        var nuevaPassword = SpectreHelper.PedirTexto("Nueva contraseña");
        var hash         = AirTicketSystem.shared.helpers.PasswordHasher.Hash(nuevaPassword);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var u = await scope.ServiceProvider.GetRequiredService<ChangePasswordUseCase>()
                .ExecuteAsync(id, hash);
            SpectreHelper.MostrarExito($"Contraseña del usuario '{u.Username.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CambiarRolAsync()
    {
        await ListarRolesAsync();
        var id    = SpectreHelper.PedirEntero("ID del usuario");
        var rolId = SpectreHelper.PedirEntero("ID del nuevo rol");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var u = await scope.ServiceProvider.GetRequiredService<ChangeRoleUseCase>()
                .ExecuteAsync(id, rolId);
            SpectreHelper.MostrarExito($"Rol del usuario '{u.Username.Valor}' actualizado a rol {u.RolId}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del usuario a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateUserUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Usuario activado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del usuario a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar el usuario?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateUserUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Usuario desactivado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ListarRolesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllRolesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla, r.Id.ToString(), r.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
