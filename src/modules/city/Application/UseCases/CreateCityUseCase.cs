// src/modules/city/Application/UseCases/CreateCityUseCase.cs
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Domain.ValueObjects;
using AirTicketSystem.modules.department.Domain.Repositories;

namespace AirTicketSystem.modules.city.Application.UseCases;

public sealed class CreateCityUseCase
{
    private readonly ICityRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;

    public CreateCityUseCase(
        ICityRepository repository,
        IDepartmentRepository departmentRepository)
    {
        _repository           = repository;
        _departmentRepository = departmentRepository;
    }

    public async Task<City> ExecuteAsync(
        int departamentoId,
        string nombre,
        string? codigoPostal,
        CancellationToken cancellationToken = default)
    {
        _ = await _departmentRepository.FindByIdAsync(departamentoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {departamentoId}.");

        var nombreVO = NombreCity.Crear(nombre);

        if (await _repository.ExistsByNombreAndDepartamentoAsync(nombreVO.Valor, departamentoId))
            throw new InvalidOperationException(
                $"Ya existe una ciudad con el nombre '{nombreVO.Valor}' " +
                $"en el departamento con ID {departamentoId}.");

        string? codigoNormalizado = codigoPostal is not null
            ? CodigoPostalCity.Crear(codigoPostal).Valor
            : null;

        var city = City.Crear(departamentoId, nombreVO.Valor, codigoNormalizado);
        await _repository.SaveAsync(city);
        return city;
    }
}
