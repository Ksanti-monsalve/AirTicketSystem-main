// src/modules/gender/Application/UseCases/GetAllGendersUseCase.cs
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Domain.Repositories;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public sealed class GetAllGendersUseCase
{
    private readonly IGenderRepository _repository;

    public GetAllGendersUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Gender>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
