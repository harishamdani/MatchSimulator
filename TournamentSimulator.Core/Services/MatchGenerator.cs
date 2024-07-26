//using TournamentSimulator.Core.Entities;
//using TournamentSimulator.Core.Interfaces;

//namespace TournamentSimulator.Core.Services;

//public class MatchGenerator : IMatchGenerator
//{
//    public List<Match> GenerateMatches(List<Team> teams)
//    {
//        if (teams?.Any() != true)
//        {
//            return new List<Match>();
//        }

//        var matches = new List<Match>();
//        var teamCount = teams.Count;
//        var roundCount = teamCount % 2 == 0 ? teamCount - 1 : teamCount;
//        var matchesPerRound = teamCount / 2;

//        // If odd number of teams, add a bye (null) team
//        if (teamCount % 2 != 0)
//        {
//            teams.Add(null);
//            teamCount++;
//        }

//        for (var round = 0; round < roundCount; round++)
//        {
//            for (var match = 0; match < matchesPerRound; match++)
//            {
//                var home = (round + match) % (teamCount - 1);
//                var away = (teamCount - 1 - match + round) % (teamCount - 1);

//                // Last team stays in the last position, others rotate
//                if (match == 0)
//                {
//                    away = teamCount - 1;
//                }

//                // Skip matches with bye team
//                if (teams[home] == null || teams[away] == null)
//                {
//                    continue;
//                }

//                var homeTeam = teams[home];
//                var awayTeam = teams[away];

//                // Alternate home and away each round for fairness
//                if (round % 2 == 1)
//                {
//                    (homeTeam, awayTeam) = (awayTeam, homeTeam);
//                }

//                matches.Add(new Match(homeTeam, awayTeam, 0, 0, round + 1));
//            }
//        }

//        // Remove the bye team if it was added
//        if (teams.Count % 2 == 0 && teams[teams.Count - 1] == null)
//        {
//            teams.RemoveAt(teams.Count - 1);
//        }

//        return matches;
//    }
//}

using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

//public class MatchGenerator : IMatchGenerator
//{
//    public List<Match> GenerateMatches(List<Team> teams)
//    {
//        var matches = new List<Match>();
//        var teamCount = teams.Count;
//        var roundCount = teamCount % 2 == 0 ? teamCount - 1 : teamCount;
//        var matchesPerRound = teamCount / 2;

//        var teamsCopy = new List<Team>(teams);
//        if (teamCount % 2 != 0)
//        {
//            teamsCopy.Add(null); // Add a bye team
//            teamCount++;
//        }

//        for (var round = 0; round < roundCount; round++)
//        {
//            for (var match = 0; match < matchesPerRound; match++)
//            {
//                var home = (round + match) % (teamCount - 1);
//                var away = (teamCount - 1 - match + round) % (teamCount - 1);

//                if (match == 0)
//                {
//                    away = teamCount - 1;
//                }

//                var homeTeam = teamsCopy[home];
//                var awayTeam = teamsCopy[away];

//                if (homeTeam != null && awayTeam != null)
//                {
//                    if (round % 2 == 1)
//                    {
//                        (homeTeam, awayTeam) = (awayTeam, homeTeam);
//                    }
//                    matches.Add(new Match(homeTeam, awayTeam, 0, 0, round + 1));
//                }
//            }

//            // Rotate teams, keeping the last team fixed
//            var firstTeam = teamsCopy[0];
//            for (int i = 0; i < teamCount - 1; i++)
//            {
//                teamsCopy[i] = teamsCopy[i + 1];
//            }
//            teamsCopy[teamCount - 2] = firstTeam;
//        }

//        return matches;
//    }
//}

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