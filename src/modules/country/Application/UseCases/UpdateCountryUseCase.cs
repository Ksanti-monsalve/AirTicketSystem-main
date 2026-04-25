// src/modules/country/Application/UseCases/UpdateCountryUseCase.cs
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Domain.ValueObjects;

namespace AirTicketSystem.modules.country.Application.UseCases;

public sealed class UpdateCountryUseCase
{
    private readonly ICountryRepository _repository;

    public UpdateCountryUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Country> ExecuteAsync(
        int id,
        string nombre,
        string codigoIso2,
        string codigoIso3,
        CancellationToken cancellationToken = default)
    {
        var country = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {id}.");

        var nombreVO = NombreCountry.Crear(nombre);
        var iso2VO   = CodigoIso2Country.Crear(codigoIso2);
        var iso3VO   = CodigoIso3Country.Crear(codigoIso3);

        if (!string.Equals(iso2VO.Valor, country.CodigoIso2.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByCodigoIso2Async(iso2VO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro país con el código ISO2 '{iso2VO.Valor}'.");

        if (!string.Equals(iso3VO.Valor, country.CodigoIso3.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByCodigoIso3Async(iso3VO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro país con el código ISO3 '{iso3VO.Valor}'.");

        country.ActualizarNombre(nombreVO.Valor);
        country.ActualizarCodigoIso2(iso2VO.Valor);
        country.ActualizarCodigoIso3(iso3VO.Valor);
        await _repository.UpdateAsync(country);
        return country;
    }
}
