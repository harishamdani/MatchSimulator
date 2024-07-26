using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Interfaces;

public interface IMatchGenerator
{
    List<Match> GenerateMatches(List<Team> teams);
}