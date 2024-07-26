using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Interfaces;

public interface IMatchSimulator
{
    Match SimulateMatch(Team homeTeam, Team awayTeam);

    List<Match> SimulatedMatches(Group group);
}