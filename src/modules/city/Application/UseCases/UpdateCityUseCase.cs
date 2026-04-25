// src/modules/city/Application/UseCases/UpdateCityUseCase.cs
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Domain.ValueObjects;

namespace AirTicketSystem.modules.city.Application.UseCases;

public sealed class UpdateCityUseCase
{
    private readonly ICityRepository _repository;

    public UpdateCityUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<City> ExecuteAsync(
        int id,
        string nombre,
        string? codigoPostal,
        CancellationToken cancellationToken = default)
    {
        var city = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {id}.");

        var nombreVO = NombreCity.Crear(nombre);

        if (!string.Equals(nombreVO.Valor, city.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAndDepartamentoAsync(nombreVO.Valor, city.DepartamentoId))
            throw new InvalidOperationException(
                $"Ya existe otra ciudad con el nombre '{nombreVO.Valor}' en el mismo departamento.");

        city.ActualizarNombre(nombreVO.Valor);
        city.ActualizarCodigoPostal(codigoPostal is not null
            ? CodigoPostalCity.Crear(codigoPostal).Valor
            : null);
        await _repository.UpdateAsync(city);
        return city;
    }
}
