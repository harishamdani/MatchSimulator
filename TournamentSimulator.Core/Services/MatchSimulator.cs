using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class MatchSimulator : IMatchSimulator
{
    private readonly Random _random = new();

    public Match SimulateMatch(Team homeTeam, Team awayTeam)
    {
        var homeGoals = SimulateGoals(homeTeam.Strength);
        var awayGoals = SimulateGoals(awayTeam.Strength);

        return new Match(homeTeam, awayTeam, homeGoals, awayGoals, 0);
    }

    public List<Match> SimulatedMatches(Group group) =>
        group.Matches
            .Select(match => SimulateMatch(match.HomeTeam, match.AwayTeam))
            .ToList();

    private int SimulateGoals(int teamStrength)
    {
        var baseChance = teamStrength / 100.0;
        var weights = new[] { 1.0, 1.0, 0.8, 0.6, 0.4 };
        var adjustedWeights = weights.Select(w => w * baseChance).ToArray();

        var totalWeight = adjustedWeights.Sum();
        var randomValue = _random.NextDouble() * totalWeight;

        var cumulativeWeight = 0.0;
        for (var i = 0; i < adjustedWeights.Length; i++)
        {
            cumulativeWeight += adjustedWeights[i];
            if (randomValue <= cumulativeWeight)
            {
                return i;
            }
        }

        return adjustedWeights.Length - 1;
    }
}