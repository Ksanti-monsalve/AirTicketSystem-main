// src/modules/department/Application/UseCases/GetAllDepartmentsUseCase.cs
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;

namespace AirTicketSystem.modules.department.Application.UseCases;

public sealed class GetAllDepartmentsUseCase
{
    private readonly IDepartmentRepository _repository;

    public GetAllDepartmentsUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Department>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
