// src/modules/boardingpass/Application/Services/BoardingPassService.cs
using AirTicketSystem.modules.boardingpass.Application.Interfaces;
using AirTicketSystem.modules.boardingpass.Application.UseCases;
using AirTicketSystem.modules.boardingpass.Domain.aggregate;

namespace AirTicketSystem.modules.boardingpass.Application.Services;

public sealed class BoardingPassService : IBoardingPassService
{
    private readonly GenerateBoardingPassUseCase  _generate;
    private readonly GetBoardingPassByCodeUseCase _getByCode;
    private readonly AssignGateUseCase            _assignGate;
    private readonly AssignBoardingTimeUseCase    _assignTime;

    public BoardingPassService(
        GenerateBoardingPassUseCase  generate,
        GetBoardingPassByCodeUseCase getByCode,
        AssignGateUseCase            assignGate,
        AssignBoardingTimeUseCase    assignTime)
    {
        _generate   = generate;
        _getByCode  = getByCode;
        _assignGate = assignGate;
        _assignTime = assignTime;
    }

    public Task<BoardingPass> GenerateAsync(
        int checkinId,
        string numeroVuelo,
        string codigoAsiento,
        DateTime fechaSalidaVuelo,
        int? puertaEmbarqueId,
        DateTime? horaEmbarque)
        => _generate.ExecuteAsync(
            checkinId, numeroVuelo, codigoAsiento,
            fechaSalidaVuelo, puertaEmbarqueId, horaEmbarque);

    public Task<BoardingPass> GetByCodeAsync(string codigoPase)
        => _getByCode.ExecuteAsync(codigoPase);

    public Task<BoardingPass> AssignGateAsync(int id, int puertaEmbarqueId)
        => _assignGate.ExecuteAsync(id, puertaEmbarqueId);

    public Task<BoardingPass> AssignBoardingTimeAsync(
        int id, DateTime horaEmbarque, DateTime fechaSalidaVuelo)
        => _assignTime.ExecuteAsync(id, horaEmbarque, fechaSalidaVuelo);
}
