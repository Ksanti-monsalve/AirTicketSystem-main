// src/modules/department/Application/UseCases/GetDepartmentByIdUseCase.cs
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;

namespace AirTicketSystem.modules.department.Application.UseCases;

public sealed class GetDepartmentByIdUseCase
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentByIdUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Department> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {id}.");
    }
}
