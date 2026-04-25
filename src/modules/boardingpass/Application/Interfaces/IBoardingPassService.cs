// src/modules/boardingpass/Application/Interfaces/IBoardingPassService.cs
using AirTicketSystem.modules.boardingpass.Domain.aggregate;

namespace AirTicketSystem.modules.boardingpass.Application.Interfaces;

public interface IBoardingPassService
{
    Task<BoardingPass> GenerateAsync(
        int checkinId,
        string numeroVuelo,
        string codigoAsiento,
        DateTime fechaSalidaVuelo,
        int? puertaEmbarqueId,
        DateTime? horaEmbarque);
    Task<BoardingPass> GetByCodeAsync(string codigoPase);
    Task<BoardingPass> AssignGateAsync(int id, int puertaEmbarqueId);
    Task<BoardingPass> AssignBoardingTimeAsync(int id, DateTime horaEmbarque, DateTime fechaSalidaVuelo);
}
