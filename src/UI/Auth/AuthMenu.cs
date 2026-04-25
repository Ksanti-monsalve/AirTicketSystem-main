// src/UI/Auth/AuthMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.shared.UI;
using AirTicketSystem.modules.user.Application.UseCases;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Domain.aggregate;

namespace AirTicketSystem.UI.Auth;

public sealed class AuthMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public AuthMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task<bool> MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("✈  Air Ticket System");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "Bienvenido — seleccione una opción",
                ["Iniciar sesión", "Registrarse como cliente", "Salir"]);

            switch (opcion)
            {
                case "Iniciar sesión":
                    if (await LoginAsync()) return true;
                    break;

                case "Registrarse como cliente":
                    await RegistrarseAsync();
                    break;

                case "Salir":
                    return false;
            }
        }
    }

    // ── Login ─────────────────────────────────────────────────────────

    private async Task<bool> LoginAsync()
    {
        SpectreHelper.MostrarTitulo("Iniciar Sesión");

        var username = SpectreHelper.PedirTexto("Usuario");
        var password = SpectreHelper.PedirPassword("Contraseña");
        var hash     = PasswordHasher.Hash(password);

        try
        {
            await using var scope = _provider.CreateAsyncScope();
            var useCase  = scope.ServiceProvider.GetRequiredService<LoginUseCase>();
            var roleRepo = scope.ServiceProvider.GetRequiredService<IRoleRepository>();

            var user = await useCase.ExecuteAsync(username, hash);
            var role = await roleRepo.FindByIdAsync(user.RolId)
                ?? throw new InvalidOperationException("Rol no encontrado.");

            _session.Login(user, role.Nombre.Valor);

            SpectreHelper.MostrarExito($"Bienvenido, {user.Username.Valor}!");
            SpectreHelper.EsperarTecla();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            SpectreHelper.MostrarError("Credenciales incorrectas.");
            SpectreHelper.EsperarTecla();
            return false;
        }
        catch (KeyNotFoundException ex)
        {
            SpectreHelper.MostrarError(ex.Message);
            SpectreHelper.EsperarTecla();
            return false;
        }
        catch (InvalidOperationException ex)
        {
            SpectreHelper.MostrarError(ex.Message);
            SpectreHelper.EsperarTecla();
            return false;
        }
    }

    // ── Registro ──────────────────────────────────────────────────────

    private async Task RegistrarseAsync()
    {
        SpectreHelper.MostrarTitulo("Registro de Cliente");

        SpectreHelper.MostrarInfo("Complete los siguientes datos para crear su cuenta.");

        var tipoDoc = await SelectorUI.SeleccionarTipoDocumentoAsync(_provider);
        if (tipoDoc is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var tipoDocId = tipoDoc.Id;
        var numeroDoc = SpectreHelper.PedirTexto("Número de documento");
        var nombres   = SpectreHelper.PedirTexto("Nombres");
        var apellidos = SpectreHelper.PedirTexto("Apellidos");
        var username  = SpectreHelper.PedirTexto("Nombre de usuario");
        var password  = SpectreHelper.PedirPassword("Contraseña");
        var confirm   = SpectreHelper.PedirPassword("Confirmar contraseña");

        if (password != confirm)
        {
            SpectreHelper.MostrarError("Las contraseñas no coinciden.");
            SpectreHelper.EsperarTecla();
            return;
        }

        var hash = PasswordHasher.Hash(password);

        try
        {
            await using var scope = _provider.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<RegisterClientUseCase>();

            await useCase.ExecuteAsync(tipoDocId, numeroDoc, nombres, apellidos,
                username, hash);

            SpectreHelper.MostrarExito(
                "Cuenta creada exitosamente. Ya puede iniciar sesión.");
        }
        catch (Exception ex)
        {
            SpectreHelper.MostrarError(ex.Message);
        }

        SpectreHelper.EsperarTecla();
    }
}
