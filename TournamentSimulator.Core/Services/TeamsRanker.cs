using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Services;

public static class TeamsRanker
{
    public static List<Team> RankTeams(List<Team> teams, List<Match> matches)
    {
        var sortedTeams = teams.OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.GoalsFor)
            .ThenBy(t => t.GoalsAgainst)
            .ThenBy(t => GetHeadToHeadRank(t, teams, matches))
            .ThenBy(t => t.Name)
            .ToList();

        for (var i = 0; i < sortedTeams.Count; i++)
        {
            sortedTeams[i].Ranking = i + 1;
        }

        return sortedTeams;
    }

    private static int GetHeadToHeadRank(Team team, List<Team> allTeams, List<Match> matches)
    {
        var tiedTeams = allTeams.Where(t => t != team &&
                                            t.Points == team.Points &&
                                            t.GoalDifference == team.GoalDifference &&
                                            t.GoalsFor == team.GoalsFor &&
                                            t.GoalsAgainst == team.GoalsAgainst).ToList();

        if (tiedTeams.Count == 0)
        {
            return 0;
        }

        var lostHeadToHead = 0;
        for (var index = 0; index < tiedTeams.Count; index++)
        {
            var opponent = tiedTeams[index];
            var headToHeadMatch = matches.FirstOrDefault(m =>
                (m.HomeTeam == team && m.AwayTeam == opponent) ||
                (m.AwayTeam == team && m.HomeTeam == opponent)
            );

            if (headToHeadMatch == null)
            {
                continue;
            }

            if ((headToHeadMatch.HomeTeam == team && headToHeadMatch.HomeGoals < headToHeadMatch.AwayGoals) ||
                (headToHeadMatch.AwayTeam == team && headToHeadMatch.AwayGoals < headToHeadMatch.HomeGoals))
            {
                lostHeadToHead++;
            }
        }

        return lostHeadToHead;
    }
}