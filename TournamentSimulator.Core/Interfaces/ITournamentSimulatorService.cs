using TournamentSimulator.Core.Entities.Results;

namespace TournamentSimulator.Core.Interfaces;

public interface ITournamentSimulatorService
{
    TournamentSimulationResult SimulateGroup(int numberOfTeams, int numberOfQualifiedTeams);
}