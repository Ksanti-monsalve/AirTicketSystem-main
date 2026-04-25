// src/modules/boardingpass/Application/UseCases/GetBoardingPassByCodeUseCase.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;

namespace AirTicketSystem.modules.boardingpass.Application.UseCases;

public sealed class GetBoardingPassByCodeUseCase
{
    private readonly IBoardingPassRepository _repository;

    public GetBoardingPassByCodeUseCase(IBoardingPassRepository repository) => _repository = repository;

    public async Task<BoardingPass> ExecuteAsync(
        string codigoPase, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codigoPase))
            throw new ArgumentException("El código del pase de abordar no puede estar vacío.");

        return await _repository.FindByCodigoPaseAsync(codigoPase)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pase de abordar con código '{codigoPase}'.");
    }
}
