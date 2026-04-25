// src/modules/airline/Application/Services/AirlineService.cs
using AirTicketSystem.modules.airline.Application.Interfaces;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Application.Services;

public sealed class AirlineService : IAirlineService
{
    private readonly CreateAirlineUseCase        _create;
    private readonly GetAirlineByIdUseCase       _getById;
    private readonly GetAirlineByIataUseCase     _getByIata;
    private readonly GetAllAirlinesUseCase       _getAll;
    private readonly GetActiveAirlinesUseCase    _getActivas;
    private readonly UpdateAirlineUseCase        _update;
    private readonly ActivateAirlineUseCase      _activate;
    private readonly DeactivateAirlineUseCase    _deactivate;
    private readonly DeleteAirlineUseCase        _delete;
    private readonly AddAirlinePhoneUseCase      _addPhone;
    private readonly RemoveAirlinePhoneUseCase   _removePhone;
    private readonly AddAirlineEmailUseCase      _addEmail;
    private readonly RemoveAirlineEmailUseCase   _removeEmail;

    public AirlineService(
        CreateAirlineUseCase      create,
        GetAirlineByIdUseCase     getById,
        GetAirlineByIataUseCase   getByIata,
        GetAllAirlinesUseCase     getAll,
        GetActiveAirlinesUseCase  getActivas,
        UpdateAirlineUseCase      update,
        ActivateAirlineUseCase    activate,
        DeactivateAirlineUseCase  deactivate,
        DeleteAirlineUseCase      delete,
        AddAirlinePhoneUseCase    addPhone,
        RemoveAirlinePhoneUseCase removePhone,
        AddAirlineEmailUseCase    addEmail,
        RemoveAirlineEmailUseCase removeEmail)
    {
        _create      = create;
        _getById     = getById;
        _getByIata   = getByIata;
        _getAll      = getAll;
        _getActivas  = getActivas;
        _update      = update;
        _activate    = activate;
        _deactivate  = deactivate;
        _delete      = delete;
        _addPhone    = addPhone;
        _removePhone = removePhone;
        _addEmail    = addEmail;
        _removeEmail = removeEmail;
    }

    public Task<Airline> CreateAsync(
        int paisId, string codigoIata, string codigoIcao,
        string nombre, string? nombreComercial, string? sitioWeb)
        => _create.ExecuteAsync(
            paisId, codigoIata, codigoIcao, nombre, nombreComercial, sitioWeb);

    public Task<Airline> GetByIdAsync(int id)      => _getById.ExecuteAsync(id);
    public Task<Airline> GetByIataAsync(string iata) => _getByIata.ExecuteAsync(iata);
    public Task<IReadOnlyCollection<Airline>> GetAllAsync()    => _getAll.ExecuteAsync();
    public Task<IReadOnlyCollection<Airline>> GetActivasAsync() => _getActivas.ExecuteAsync();

    public Task<Airline> UpdateAsync(
        int id, string nombre, string? nombreComercial, string? sitioWeb)
        => _update.ExecuteAsync(id, nombre, nombreComercial, sitioWeb);

    public Task ActivateAsync(int id)   => _activate.ExecuteAsync(id);
    public Task DeactivateAsync(int id) => _deactivate.ExecuteAsync(id);
    public Task DeleteAsync(int id)     => _delete.ExecuteAsync(id);

    public Task<AirlinePhone> AddPhoneAsync(
        int aerolineaId, int tipoTelefonoId,
        string numero, string? indicativo, bool esPrincipal)
        => _addPhone.ExecuteAsync(
            aerolineaId, tipoTelefonoId, numero, indicativo, esPrincipal);

    public Task RemovePhoneAsync(int phoneId)
        => _removePhone.ExecuteAsync(phoneId);

    public Task<AirlineEmail> AddEmailAsync(
        int aerolineaId, int tipoEmailId,
        string email, bool esPrincipal)
        => _addEmail.ExecuteAsync(aerolineaId, tipoEmailId, email, esPrincipal);

    public Task RemoveEmailAsync(int emailId)
        => _removeEmail.ExecuteAsync(emailId);
}
