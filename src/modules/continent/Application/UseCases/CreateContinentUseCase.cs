// src/modules/continent/Application/UseCases/CreateContinentUseCase.cs
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Domain.ValueObjects;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public sealed class CreateContinentUseCase
{
    private readonly IContinentRepository _repository;

    public CreateContinentUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Continent> ExecuteAsync(
        string nombre,
        string codigo,
        CancellationToken cancellationToken = default)
    {
        var nombreVO = NombreContinent.Crear(nombre);
        var codigoVO = CodigoContinent.Crear(codigo);

        if (await _repository.ExistsByCodigoAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un continente con el código '{codigoVO.Valor}'.");

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un continente con el nombre '{nombreVO.Valor}'.");

        var continent = Continent.Crear(nombreVO.Valor, codigoVO.Valor);
        await _repository.SaveAsync(continent);
        return continent;
    }
}
