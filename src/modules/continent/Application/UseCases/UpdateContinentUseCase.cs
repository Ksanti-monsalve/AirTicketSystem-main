// src/modules/continent/Application/UseCases/UpdateContinentUseCase.cs
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Domain.ValueObjects;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public sealed class UpdateContinentUseCase
{
    private readonly IContinentRepository _repository;

    public UpdateContinentUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Continent> ExecuteAsync(
        int id,
        string nombre,
        string codigo,
        CancellationToken cancellationToken = default)
    {
        var continent = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un continente con ID {id}.");

        var nombreVO = NombreContinent.Crear(nombre);
        var codigoVO = CodigoContinent.Crear(codigo);

        if (!string.Equals(codigoVO.Valor, continent.Codigo.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByCodigoAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro continente con el código '{codigoVO.Valor}'.");

        if (!string.Equals(nombreVO.Valor, continent.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro continente con el nombre '{nombreVO.Valor}'.");

        continent.ActualizarNombre(nombreVO.Valor);
        continent.ActualizarCodigo(codigoVO.Valor);

        await _repository.UpdateAsync(continent);
        return continent;
    }
}
