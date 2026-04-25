// src/modules/user/Application/UseCases/SeedAdminUseCase.cs
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class SeedAdminUseCase
{
    private readonly IRoleRepository         _roleRepository;
    private readonly IUserRepository         _userRepository;
    private readonly IPersonRepository       _personRepository;
    private readonly IDocumentTypeRepository _documentTypeRepository;

    public SeedAdminUseCase(
        IRoleRepository         roleRepository,
        IUserRepository         userRepository,
        IPersonRepository       personRepository,
        IDocumentTypeRepository documentTypeRepository)
    {
        _roleRepository         = roleRepository;
        _userRepository         = userRepository;
        _personRepository       = personRepository;
        _documentTypeRepository = documentTypeRepository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Garantizar tipo de documento base (requerido por la FK de Persona)
        var tiposDoc = await _documentTypeRepository.FindAllAsync();
        var tipoDocAdmin = tiposDoc.FirstOrDefault(t => t.Descripcion.Valor == "Cédula de ciudadanía");
        if (tipoDocAdmin is null)
        {
            tipoDocAdmin = DocumentType.Crear("Cédula de ciudadanía");
            await _documentTypeRepository.SaveAsync(tipoDocAdmin);
        }

        // Garantizar rol ADMIN
        var roles = await _roleRepository.FindAllAsync();
        var rolAdmin = roles.FirstOrDefault(r => r.Nombre.Valor == "ADMIN");
        if (rolAdmin is null)
        {
            rolAdmin = Role.Crear("ADMIN");
            await _roleRepository.SaveAsync(rolAdmin);
        }

        // Garantizar rol CLIENTE
        var rolCliente = roles.FirstOrDefault(r => r.Nombre.Valor == "CLIENTE");
        if (rolCliente is null)
        {
            rolCliente = Role.Crear("CLIENTE");
            await _roleRepository.SaveAsync(rolCliente);
        }

        // Garantizar usuario admin (si ya existe, no hace nada)
        if (await _userRepository.ExistsByUsernameAsync("admin"))
            return;

        var persona = Person.Crear(
            tipoDocId: tipoDocAdmin.Id,
            numeroDoc: "0000000000",
            nombres:   "Administrador",
            apellidos: "Sistema");
        await _personRepository.SaveAsync(persona);

        var hash  = PasswordHasher.Hash("admin123");
        var admin = User.Crear(persona.Id, rolAdmin.Id, "admin", hash);
        await _userRepository.SaveAsync(admin);
    }
}
