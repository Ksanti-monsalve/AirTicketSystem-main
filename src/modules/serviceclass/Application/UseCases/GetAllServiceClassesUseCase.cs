// src/modules/serviceclass/Application/UseCases/GetAllServiceClassesUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class GetAllServiceClassesUseCase
{
    private readonly IServiceClassRepository _repository;

    public GetAllServiceClassesUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<ServiceClass>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(s => s.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}