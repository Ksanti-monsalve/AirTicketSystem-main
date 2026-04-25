// src/modules/person/Application/UseCases/CreatePersonUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class CreatePersonUseCase
{
    private readonly IPersonRepository _repository;
    private readonly IDocumentTypeRepository _documentTypeRepository;

    public CreatePersonUseCase(
        IPersonRepository repository,
        IDocumentTypeRepository documentTypeRepository)
    {
        _repository            = repository;
        _documentTypeRepository = documentTypeRepository;
    }

    public async Task<Person> ExecuteAsync(
        int tipoDocId,
        string numeroDoc,
        string nombres,
        string apellidos,
        DateOnly? fechaNacimiento = null,
        int? generoId = null,
        int? nacionalidadId = null,
        CancellationToken cancellationToken = default)
    {
        _ = await _documentTypeRepository.FindByIdAsync(tipoDocId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de documento con ID {tipoDocId}.");

        if (await _repository.ExistsByDocumentoAsync(tipoDocId, numeroDoc))
            throw new InvalidOperationException(
                $"Ya existe una persona con el documento '{numeroDoc}'.");

        var person = Person.Crear(
            tipoDocId, numeroDoc, nombres, apellidos,
            fechaNacimiento, generoId, nacionalidadId);

        await _repository.SaveAsync(person);
        return person;
    }
}
