using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Interfaces;

public interface ITeamGenerator
{
    List<Team> GenerateTeams(int numberOfTeams);
}