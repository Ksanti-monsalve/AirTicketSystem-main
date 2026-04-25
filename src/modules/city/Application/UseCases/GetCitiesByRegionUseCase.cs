// src/modules/city/Application/UseCases/GetCitiesByDepartmentUseCase.cs
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;

namespace AirTicketSystem.modules.city.Application.UseCases;

public sealed class GetCitiesByDepartmentUseCase
{
    private readonly ICityRepository _repository;

    public GetCitiesByDepartmentUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<City>> ExecuteAsync(
        int departamentoId,
        CancellationToken cancellationToken = default)
    {
        if (departamentoId <= 0)
            throw new ArgumentException("El ID del departamento no es válido.");

        return await _repository.FindByDepartamentoAsync(departamentoId);
    }
}
