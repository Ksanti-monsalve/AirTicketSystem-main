// src/modules/person/Application/UseCases/UpdatePersonUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class UpdatePersonUseCase
{
    private readonly IPersonRepository _repository;

    public UpdatePersonUseCase(IPersonRepository repository) => _repository = repository;

    public async Task<Person> ExecuteAsync(
        int id,
        string nombres,
        string apellidos,
        DateOnly? fechaNacimiento,
        int? generoId,
        int? nacionalidadId,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        var person = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {id}.");

        person.ActualizarNombre(nombres, apellidos);
        person.ActualizarFechaNacimiento(fechaNacimiento);
        person.ActualizarGenero(generoId);
        person.ActualizarNacionalidad(nacionalidadId);

        await _repository.UpdateAsync(person);
        return person;
    }
}
