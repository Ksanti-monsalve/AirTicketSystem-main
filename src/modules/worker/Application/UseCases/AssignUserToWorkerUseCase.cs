// src/modules/worker/Application/UseCases/AssignUserToWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class AssignUserToWorkerUseCase
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IUserRepository   _userRepository;

    public AssignUserToWorkerUseCase(
        IWorkerRepository workerRepository,
        IUserRepository userRepository)
    {
        _workerRepository = workerRepository;
        _userRepository   = userRepository;
    }

    public async Task<Worker> ExecuteAsync(
        int trabajadorId, int usuarioId, CancellationToken cancellationToken = default)
    {
        var worker = await _workerRepository.FindByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        var usuario = await _userRepository.FindByIdAsync(usuarioId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {usuarioId}.");

        if (usuario.EstaBloqueado)
            throw new InvalidOperationException(
                "No se puede asignar un usuario inactivo o bloqueado a un trabajador.");

        worker.AsignarUsuario(usuarioId);
        await _workerRepository.UpdateAsync(worker);
        return worker;
    }
}
