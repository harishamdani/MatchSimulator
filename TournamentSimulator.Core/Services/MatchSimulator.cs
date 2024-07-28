using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class MatchSimulator : IMatchSimulator
{
    private readonly Random _random = new();
    private const double HomeAdvantage = 1.3; // 30% advantage for home team
    private const double AwayDisadvantage = 0.8; // 20% disadvantage for away team

    public Match SimulateMatch(Team homeTeam, Team awayTeam)
    {
        var homeGoals = SimulateGoals(homeTeam.Strength, true);
        var awayGoals = SimulateGoals(awayTeam.Strength, false);

        return new Match(homeTeam, awayTeam, homeGoals, awayGoals, 0);
    }

    public List<Match> SimulatedMatches(Group group) =>
        group.Matches
            .Select(match => SimulateMatch(match.HomeTeam, match.AwayTeam))
            .ToList();

    private int SimulateGoals(int teamStrength, bool isHome)
    {
        var baseChance = teamStrength / 100.0;
        var adjustedChance = isHome ? baseChance * HomeAdvantage : baseChance * AwayDisadvantage;

        var weights = new[] { 1.0, 1.0, 0.8, 0.6, 0.4, 0.3, 0.2 }; // Extended to allow for more goals
        var adjustedWeights = weights.Select(w => w * adjustedChance).ToArray();

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