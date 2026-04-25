// src/modules/user/Application/UseCases/RegisterClientUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.client.Application.UseCases;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class RegisterClientUseCase
{
    private readonly IUserRepository   _userRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IRoleRepository   _roleRepository;
    private readonly CreateClientUseCase _createClientUseCase;

    public RegisterClientUseCase(
        IUserRepository   userRepository,
        IPersonRepository personRepository,
        IRoleRepository   roleRepository,
        CreateClientUseCase createClientUseCase)
    {
        _userRepository   = userRepository;
        _personRepository = personRepository;
        _roleRepository   = roleRepository;
        _createClientUseCase = createClientUseCase;
    }

    public async Task<User> ExecuteAsync(
        int    tipoDocId,
        string numeroDoc,
        string nombres,
        string apellidos,
        string username,
        string passwordHash,
        DateOnly? fechaNacimiento = null,
        CancellationToken cancellationToken = default)
    {
        if (await _personRepository.ExistsByDocumentoAsync(tipoDocId, numeroDoc))
            throw new InvalidOperationException(
                $"Ya existe una persona registrada con ese documento.");

        if (await _userRepository.ExistsByUsernameAsync(username))
            throw new InvalidOperationException(
                $"El nombre de usuario '{username}' ya está en uso.");

        var roles = await _roleRepository.FindAllAsync();
        var rolCliente = roles.FirstOrDefault(r => r.Nombre.Valor == "CLIENTE")
            ?? throw new InvalidOperationException(
                "No se encontró el rol CLIENTE en el sistema. " +
                "Contacte al administrador.");

        var persona = Person.Crear(tipoDocId, numeroDoc, nombres, apellidos,
            fechaNacimiento);
        await _personRepository.SaveAsync(persona);

        var user = User.Crear(persona.Id, rolCliente.Id, username, passwordHash);
        await _userRepository.SaveAsync(user);

        // Crear el perfil de cliente asociado (requerido para portal de clientes)
        _ = await _createClientUseCase.ExecuteAsync(persona.Id, user.Id, cancellationToken);

        return user;
    }
}
