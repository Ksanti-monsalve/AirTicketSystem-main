// src/modules/person/Application/Interfaces/IPersonService.cs
using AirTicketSystem.modules.person.Domain.aggregate;

namespace AirTicketSystem.modules.person.Application.Interfaces;

public interface IPersonService
{
    Task<Person> CreateAsync(
        int tipoDocId, string numeroDoc, string nombres, string apellidos,
        DateOnly? fechaNacimiento, int? generoId, int? nacionalidadId);
    Task<Person> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Person>> GetAllAsync();
    Task<Person> GetByDocumentAsync(int tipoDocId, string numeroDoc);
    Task<Person> UpdateAsync(
        int id, string nombres, string apellidos,
        DateOnly? fechaNacimiento, int? generoId, int? nacionalidadId);
    Task DeleteAsync(int id);

    // Teléfonos
    Task<PersonPhone> AddPhoneAsync(
        int personaId, int tipoTelefonoId, string numero,
        string? indicativoPais, bool esPrincipal);
    Task<PersonPhone> SetPrincipalPhoneAsync(int phoneId);
    Task DeletePhoneAsync(int phoneId);

    // Emails
    Task<PersonEmail> AddEmailAsync(
        int personaId, int tipoEmailId, string email, bool esPrincipal);
    Task<PersonEmail> SetPrincipalEmailAsync(int emailId);
    Task DeleteEmailAsync(int emailId);

    // Direcciones
    Task<PersonAddress> AddAddressAsync(
        int personaId, int tipoDireccionId, int ciudadId,
        string direccionLinea1, string? direccionLinea2,
        string? codigoPostal, bool esPrincipal);
    Task<PersonAddress> SetPrincipalAddressAsync(int addressId);
    Task DeleteAddressAsync(int addressId);
}
