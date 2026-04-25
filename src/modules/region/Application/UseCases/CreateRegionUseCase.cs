// src/modules/region/Application/UseCases/CreateRegionUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Domain.ValueObjects;

namespace AirTicketSystem.modules.region.Application.UseCases;

public sealed class CreateRegionUseCase
{
    private readonly IRegionRepository _repository;
    private readonly ICountryRepository _countryRepository;

    public CreateRegionUseCase(
        IRegionRepository repository,
        ICountryRepository countryRepository)
    {
        _repository        = repository;
        _countryRepository = countryRepository;
    }

    public async Task<Region> ExecuteAsync(
        int paisId,
        string nombre,
        string? codigo,
        CancellationToken cancellationToken = default)
    {
        _ = await _countryRepository.FindByIdAsync(paisId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {paisId}.");

        var nombreVO = NombreRegion.Crear(nombre);

        if (await _repository.ExistsByNombreAndPaisAsync(nombreVO.Valor, paisId))
            throw new InvalidOperationException(
                $"Ya existe una región con el nombre '{nombreVO.Valor}' " +
                $"en el país con ID {paisId}.");

        string? codigoNormalizado = codigo is not null
            ? CodigoRegion.Crear(codigo).Valor
            : null;

        var region = Region.Crear(paisId, nombreVO.Valor, codigoNormalizado);
        await _repository.SaveAsync(region);
        return region;
    }
}
