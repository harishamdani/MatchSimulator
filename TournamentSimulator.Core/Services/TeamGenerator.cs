using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class TeamGenerator : ITeamGenerator
{
    private readonly Random _random = new();

    public List<Team> GenerateTeams(int numberOfTeams)
    {
        var teams = new List<Team>();
        for (var i = 0; i < numberOfTeams; i++)
        {
            var strength = _random.Next(1, 101); // Random strength between 1 and 100
            teams.Add(new Team($"Team {i + 1}", strength));
        }
        return teams;
    }
}