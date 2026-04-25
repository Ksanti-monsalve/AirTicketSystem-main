// src/modules/client/Application/Services/ClientService.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Application.Interfaces;
using AirTicketSystem.modules.client.Application.UseCases;

namespace AirTicketSystem.modules.client.Application.Services;

public sealed class ClientService : IClientService
{
    private readonly CreateClientUseCase                  _create;
    private readonly GetClientByIdUseCase                 _getById;
    private readonly GetAllClientsUseCase                 _getAll;
    private readonly GetActiveClientsUseCase              _getActivos;
    private readonly ActivateClientUseCase                _activate;
    private readonly DeactivateClientUseCase              _deactivate;
    private readonly DeleteClientUseCase                  _delete;
    private readonly AddEmergencyContactUseCase           _addContact;
    private readonly SetPrincipalEmergencyContactUseCase  _setPrincipalContact;
    private readonly UpdateEmergencyContactUseCase        _updateContact;
    private readonly DeleteEmergencyContactUseCase        _deleteContact;

    public ClientService(
        CreateClientUseCase create,
        GetClientByIdUseCase getById,
        GetAllClientsUseCase getAll,
        GetActiveClientsUseCase getActivos,
        ActivateClientUseCase activate,
        DeactivateClientUseCase deactivate,
        DeleteClientUseCase delete,
        AddEmergencyContactUseCase addContact,
        SetPrincipalEmergencyContactUseCase setPrincipalContact,
        UpdateEmergencyContactUseCase updateContact,
        DeleteEmergencyContactUseCase deleteContact)
    {
        _create              = create;
        _getById             = getById;
        _getAll              = getAll;
        _getActivos          = getActivos;
        _activate            = activate;
        _deactivate          = deactivate;
        _delete              = delete;
        _addContact          = addContact;
        _setPrincipalContact = setPrincipalContact;
        _updateContact       = updateContact;
        _deleteContact       = deleteContact;
    }

    public Task<Client> CreateAsync(int personaId, int usuarioId)
        => _create.ExecuteAsync(personaId, usuarioId);

    public Task<Client> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Client>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Client>> GetActivosAsync()
        => _getActivos.ExecuteAsync();

    public Task<Client> ActivateAsync(int id)
        => _activate.ExecuteAsync(id);

    public Task<Client> DeactivateAsync(int id)
        => _deactivate.ExecuteAsync(id);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);

    public Task<EmergencyContact> AddEmergencyContactAsync(
        int clienteId, int personaId, int relacionId, bool esPrincipal)
        => _addContact.ExecuteAsync(clienteId, personaId, relacionId, esPrincipal);

    public Task<EmergencyContact> SetPrincipalEmergencyContactAsync(int contactId)
        => _setPrincipalContact.ExecuteAsync(contactId);

    public Task<EmergencyContact> UpdateEmergencyContactAsync(int contactId, int relacionId)
        => _updateContact.ExecuteAsync(contactId, relacionId);

    public Task DeleteEmergencyContactAsync(int contactId)
        => _deleteContact.ExecuteAsync(contactId);
}
