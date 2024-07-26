using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class GroupSimulator(IMatchSimulator matchSimulator) : IGroupSimulator
{
    public Group SimulateGroup(Group group)
    {
        var simulatedMatches = matchSimulator.SimulatedMatches(group);
        var updatedTeams = CalculateTeams(group.Teams, simulatedMatches);
        var sortedTeams = TeamsRanker.RankTeams(updatedTeams, simulatedMatches);

        return new Group(sortedTeams, simulatedMatches);
    }

    private static List<Team> CalculateTeams(List<Team> teams, List<Match> matches) =>
        teams
            .Select(team => CalculateTeam(team, matches))
            .ToList();

    private static Team CalculateTeam(Team team, List<Match> matches)
    {
        var teamMatches = GetTeamMatches(team, matches);

        for (var i = 0; i < teamMatches.Count; i++)
        {
            var isHome = teamMatches[i].HomeTeam.Name == team.Name;
            var teamGoals = isHome ? teamMatches[i].HomeGoals : teamMatches[i].AwayGoals;
            var opponentGoals = isHome ? teamMatches[i].AwayGoals : teamMatches[i].HomeGoals;

            team.GoalsFor += teamGoals;
            team.GoalsAgainst += opponentGoals;

            if (teamGoals > opponentGoals)
            {
                team.Points += 3;
                team.Wins++;
            }
            else if (teamGoals == opponentGoals)
            {
                team.Points += 1;
                team.Draws++;
            }
            else
            {
                team.Losses++;
            }
        }

        return team;
    }

    private static List<Match> GetTeamMatches(Team team, List<Match> matches)
    {
        return matches
            .Where(m => string.Equals(m.HomeTeam.Name,
                            team.Name,
                            StringComparison.OrdinalIgnoreCase
                        ) ||
                        string.Equals(m.AwayTeam.Name,
                            team.Name,
                            StringComparison.OrdinalIgnoreCase
                        ))
            .ToList();
    }
    
}