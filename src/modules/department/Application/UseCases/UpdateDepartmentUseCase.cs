// src/modules/department/Application/UseCases/UpdateDepartmentUseCase.cs
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Domain.ValueObjects;

namespace AirTicketSystem.modules.department.Application.UseCases;

public sealed class UpdateDepartmentUseCase
{
    private readonly IDepartmentRepository _repository;

    public UpdateDepartmentUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Department> ExecuteAsync(
        int id,
        string nombre,
        string? codigo,
        CancellationToken cancellationToken = default)
    {
        var department = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {id}.");

        var nombreVO = NombreDepartment.Crear(nombre);

        if (!string.Equals(nombreVO.Valor, department.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAndRegionAsync(nombreVO.Valor, department.RegionId))
            throw new InvalidOperationException(
                $"Ya existe otro departamento con el nombre '{nombreVO.Valor}' en la misma región.");

        department.ActualizarNombre(nombreVO.Valor);
        department.ActualizarCodigo(codigo is not null ? CodigoDepartment.Crear(codigo).Valor : null);
        await _repository.UpdateAsync(department);
        return department;
    }
}
