// src/modules/specialty/Application/UseCases/GetAllSpecialtiesUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.aggregate;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class GetAllSpecialtiesUseCase
{
    private readonly ISpecialtyRepository _repository;

    public GetAllSpecialtiesUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Specialty>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(s => s.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}