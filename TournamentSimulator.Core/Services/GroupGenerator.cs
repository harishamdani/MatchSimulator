using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class GroupGenerator(IMatchGenerator matchGenerator) : IGroupGenerator
{
    public Group GenerateGroup(List<Team> teams)
    {
        if (teams.Count < 2)
        {
            throw new ArgumentException("At least two teams are required to generate a group.");
        }

        var matches = matchGenerator.GenerateMatches(teams);
        return new Group(teams, matches);
    }
}