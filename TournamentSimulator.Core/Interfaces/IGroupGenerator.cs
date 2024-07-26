using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Interfaces;

public interface IGroupGenerator
{
    Group GenerateGroup(List<Team> teams);
}