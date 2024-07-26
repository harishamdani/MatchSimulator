using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class MatchGenerator : IMatchGenerator
{
    public List<Match> GenerateMatches(List<Team> teams)
    {
        if (teams.Count < 2)
        {
            return [];
        }

        var matches = new List<Match>();
        var teamCount = teams.Count;
        var roundCount = teamCount - 1;
        var halfSize = teamCount / 2;

        var teamsCopy = new List<Team>(teams);
        if (IsOddTeams(teamCount))
        {
            teamsCopy.Add(null); // Add a bye team
            teamCount++;
            roundCount = teamCount - 1;
            halfSize = teamCount / 2;
        }

        for (int round = 0; round < roundCount; round++)
        {
            for (int i = 0; i < halfSize; i++)
            {
                var homeTeam = teamsCopy[i];
                var awayTeam = teamsCopy[teamCount - 1 - i];

                if (IsByeMatch(homeTeam, awayTeam))
                {
                    continue;
                }

                if (round % 2 == 1 && i == 0)
                {
                    // Swap for the first match of odd rounds
                    matches.Add(new Match(awayTeam, homeTeam, 0, 0, round + 1));
                }
                else
                {
                    matches.Add(new Match(homeTeam, awayTeam, 0, 0, round + 1));
                }
            }

            // Rotate teams
            teamsCopy.Insert(1, teamsCopy[teamCount - 1]);
            teamsCopy.RemoveAt(teamCount);
        }

        return matches;
    }

    private static bool IsOddTeams(int teamCount) { return teamCount % 2 != 0; }

    private static bool IsByeMatch(Team homeTeam, Team awayTeam) { return homeTeam == null || awayTeam == null; }
}