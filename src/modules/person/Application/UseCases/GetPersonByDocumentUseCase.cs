// src/modules/person/Application/UseCases/GetPersonByDocumentUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class GetPersonByDocumentUseCase
{
    private readonly IPersonRepository _repository;

    public GetPersonByDocumentUseCase(IPersonRepository repository)
        => _repository = repository;

    public async Task<Person> ExecuteAsync(
        int tipoDocId,
        string numeroDoc,
        CancellationToken cancellationToken = default)
    {
        if (tipoDocId <= 0)
            throw new ArgumentException("El tipo de documento no es válido.");

        if (string.IsNullOrWhiteSpace(numeroDoc))
            throw new ArgumentException("El número de documento no puede estar vacío.");

        return await _repository.FindByDocumentoAsync(tipoDocId, numeroDoc)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con el documento '{numeroDoc}'.");
    }
}
