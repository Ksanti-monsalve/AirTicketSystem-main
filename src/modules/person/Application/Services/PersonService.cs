// src/modules/person/Application/Services/PersonService.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Application.Interfaces;
using AirTicketSystem.modules.person.Application.UseCases;

namespace AirTicketSystem.modules.person.Application.Services;

public sealed class PersonService : IPersonService
{
    private readonly CreatePersonUseCase          _create;
    private readonly GetPersonByIdUseCase         _getById;
    private readonly GetAllPersonsUseCase         _getAll;
    private readonly GetPersonByDocumentUseCase   _getByDocument;
    private readonly UpdatePersonUseCase          _update;
    private readonly DeletePersonUseCase          _delete;
    private readonly AddPersonPhoneUseCase        _addPhone;
    private readonly SetPrincipalPersonPhoneUseCase _setPrincipalPhone;
    private readonly DeletePersonPhoneUseCase     _deletePhone;
    private readonly AddPersonEmailUseCase        _addEmail;
    private readonly SetPrincipalPersonEmailUseCase _setPrincipalEmail;
    private readonly DeletePersonEmailUseCase     _deleteEmail;
    private readonly AddPersonAddressUseCase      _addAddress;
    private readonly SetPrincipalPersonAddressUseCase _setPrincipalAddress;
    private readonly DeletePersonAddressUseCase   _deleteAddress;

    public PersonService(
        CreatePersonUseCase create,
        GetPersonByIdUseCase getById,
        GetAllPersonsUseCase getAll,
        GetPersonByDocumentUseCase getByDocument,
        UpdatePersonUseCase update,
        DeletePersonUseCase delete,
        AddPersonPhoneUseCase addPhone,
        SetPrincipalPersonPhoneUseCase setPrincipalPhone,
        DeletePersonPhoneUseCase deletePhone,
        AddPersonEmailUseCase addEmail,
        SetPrincipalPersonEmailUseCase setPrincipalEmail,
        DeletePersonEmailUseCase deleteEmail,
        AddPersonAddressUseCase addAddress,
        SetPrincipalPersonAddressUseCase setPrincipalAddress,
        DeletePersonAddressUseCase deleteAddress)
    {
        _create             = create;
        _getById            = getById;
        _getAll             = getAll;
        _getByDocument      = getByDocument;
        _update             = update;
        _delete             = delete;
        _addPhone           = addPhone;
        _setPrincipalPhone  = setPrincipalPhone;
        _deletePhone        = deletePhone;
        _addEmail           = addEmail;
        _setPrincipalEmail  = setPrincipalEmail;
        _deleteEmail        = deleteEmail;
        _addAddress         = addAddress;
        _setPrincipalAddress = setPrincipalAddress;
        _deleteAddress      = deleteAddress;
    }

    public Task<Person> CreateAsync(
        int tipoDocId, string numeroDoc, string nombres, string apellidos,
        DateOnly? fechaNacimiento, int? generoId, int? nacionalidadId)
        => _create.ExecuteAsync(tipoDocId, numeroDoc, nombres, apellidos,
            fechaNacimiento, generoId, nacionalidadId);

    public Task<Person> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Person>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<Person> GetByDocumentAsync(int tipoDocId, string numeroDoc)
        => _getByDocument.ExecuteAsync(tipoDocId, numeroDoc);

    public Task<Person> UpdateAsync(
        int id, string nombres, string apellidos,
        DateOnly? fechaNacimiento, int? generoId, int? nacionalidadId)
        => _update.ExecuteAsync(id, nombres, apellidos, fechaNacimiento, generoId, nacionalidadId);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);

    public Task<PersonPhone> AddPhoneAsync(
        int personaId, int tipoTelefonoId, string numero,
        string? indicativoPais, bool esPrincipal)
        => _addPhone.ExecuteAsync(personaId, tipoTelefonoId, numero, indicativoPais, esPrincipal);

    public Task<PersonPhone> SetPrincipalPhoneAsync(int phoneId)
        => _setPrincipalPhone.ExecuteAsync(phoneId);

    public Task DeletePhoneAsync(int phoneId)
        => _deletePhone.ExecuteAsync(phoneId);

    public Task<PersonEmail> AddEmailAsync(
        int personaId, int tipoEmailId, string email, bool esPrincipal)
        => _addEmail.ExecuteAsync(personaId, tipoEmailId, email, esPrincipal);

    public Task<PersonEmail> SetPrincipalEmailAsync(int emailId)
        => _setPrincipalEmail.ExecuteAsync(emailId);

    public Task DeleteEmailAsync(int emailId)
        => _deleteEmail.ExecuteAsync(emailId);

    public Task<PersonAddress> AddAddressAsync(
        int personaId, int tipoDireccionId, int ciudadId,
        string direccionLinea1, string? direccionLinea2,
        string? codigoPostal, bool esPrincipal)
        => _addAddress.ExecuteAsync(personaId, tipoDireccionId, ciudadId,
            direccionLinea1, direccionLinea2, codigoPostal, esPrincipal);

    public Task<PersonAddress> SetPrincipalAddressAsync(int addressId)
        => _setPrincipalAddress.ExecuteAsync(addressId);

    public Task DeleteAddressAsync(int addressId)
        => _deleteAddress.ExecuteAsync(addressId);
}
