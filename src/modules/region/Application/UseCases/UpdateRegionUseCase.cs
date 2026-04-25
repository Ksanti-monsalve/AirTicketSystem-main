// src/modules/region/Application/UseCases/UpdateRegionUseCase.cs
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Domain.ValueObjects;

namespace AirTicketSystem.modules.region.Application.UseCases;

public sealed class UpdateRegionUseCase
{
    private readonly IRegionRepository _repository;

    public UpdateRegionUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Region> ExecuteAsync(
        int id,
        string nombre,
        string? codigo,
        CancellationToken cancellationToken = default)
    {
        var region = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {id}.");

        var nombreVO = NombreRegion.Crear(nombre);

        if (!string.Equals(nombreVO.Valor, region.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAndPaisAsync(nombreVO.Valor, region.PaisId))
            throw new InvalidOperationException(
                $"Ya existe otra región con el nombre '{nombreVO.Valor}' en el mismo país.");

        region.ActualizarNombre(nombreVO.Valor);
        region.ActualizarCodigo(codigo is not null ? CodigoRegion.Crear(codigo).Valor : null);
        await _repository.UpdateAsync(region);
        return region;
    }
}
