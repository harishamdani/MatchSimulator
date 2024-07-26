using FluentAssertions;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Services;

namespace TournamentSimulator.UnitTests.Services;

[TestFixture]
public class TeamsRankerTests
{
    [Test]
    public void RankTeams_WithSamePointsAndDifferentGoalDifference_ShouldRankCorrectly()
    {
        // Arrange
        var teamA = new Team("Team A", 50) { Points = 6, GoalsFor = 5, GoalsAgainst = 4 };
        var teamB = new Team("Team B", 50) { Points = 6, GoalsFor = 6, GoalsAgainst = 3 };
        var teamC = new Team("Team C", 50) { Points = 3, GoalsFor = 5, GoalsAgainst = 5 };
        var teamD = new Team("Team D", 50) { Points = 3, GoalsFor = 2, GoalsAgainst = 6 };

        var teams = new List<Team> { teamA, teamB, teamC, teamD };

        var matches = new List<Match>
        {
            new Match(teamA, teamB, 2, 1, 1),
            new Match(teamC, teamD, 3, 0, 1),
            new Match(teamA, teamC, 2, 1, 2),
            new Match(teamB, teamD, 3, 0, 2),
            new Match(teamA, teamD, 1, 2, 3),
            new Match(teamB, teamC, 2, 1, 3)
        };

        // Act
        var rankedTeams = TeamsRanker.RankTeams(teams, matches);

        // Assert
        rankedTeams[0].Should().Be(teamB);
        rankedTeams[1].Should().Be(teamA);
        rankedTeams[2].Should().Be(teamC);
        rankedTeams[3].Should().Be(teamD);

        rankedTeams[0].Ranking.Should().Be(1);
        rankedTeams[1].Ranking.Should().Be(2);
        rankedTeams[2].Ranking.Should().Be(3);
        rankedTeams[3].Ranking.Should().Be(4);

        // Verify tiebreakers
        rankedTeams[0].Points.Should().Be(rankedTeams[1].Points);
        rankedTeams[0].GoalDifference.Should().BeGreaterThan(rankedTeams[1].GoalDifference);
        rankedTeams[2].Points.Should().Be(rankedTeams[3].Points);
        rankedTeams[2].GoalDifference.Should().BeGreaterThan(rankedTeams[3].GoalDifference);
    }

    [Test]
    public void RankTeams_WithIdenticalStatsAndHeadToHeadResult_ShouldRankByHeadToHead()
    {
        // Arrange
        var teamA = new Team("Team A", 50) { Points = 6, GoalsFor = 5, GoalsAgainst = 3 };
        var teamB = new Team("Team B", 50) { Points = 6, GoalsFor = 5, GoalsAgainst = 3 };
        var teamC = new Team("Team C", 50) { Points = 3, GoalsFor = 3, GoalsAgainst = 5 };
        var teamD = new Team("Team D", 50) { Points = 3, GoalsFor = 3, GoalsAgainst = 5 };

        var teams = new List<Team> { teamA, teamB, teamC, teamD };

        var matches = new List<Match>
        {
            new Match(teamA, teamB, 2, 1, 1), // A wins head-to-head against B
            new Match(teamC, teamD, 2, 1, 1), // C wins head-to-head against D
            new Match(teamA, teamC, 2, 1, 2),
            new Match(teamB, teamD, 2, 1, 2),
            new Match(teamA, teamD, 1, 1, 3),
            new Match(teamB, teamC, 2, 1, 3)
        };

        // Act
        var rankedTeams = TeamsRanker.RankTeams(teams, matches);

        // Assert
        rankedTeams[0].Should().Be(teamA);
        rankedTeams[1].Should().Be(teamB);
        rankedTeams[2].Should().Be(teamC);
        rankedTeams[3].Should().Be(teamD);

        rankedTeams[0].Ranking.Should().Be(1);
        rankedTeams[1].Ranking.Should().Be(2);
        rankedTeams[2].Ranking.Should().Be(3);
        rankedTeams[3].Ranking.Should().Be(4);

        // Verify tiebreakers
        rankedTeams[0].Points.Should().Be(rankedTeams[1].Points);
        rankedTeams[0].GoalDifference.Should().Be(rankedTeams[1].GoalDifference);
        rankedTeams[0].GoalsFor.Should().Be(rankedTeams[1].GoalsFor);
        rankedTeams[0].GoalsAgainst.Should().Be(rankedTeams[1].GoalsAgainst);

        rankedTeams[2].Points.Should().Be(rankedTeams[3].Points);
        rankedTeams[2].GoalDifference.Should().Be(rankedTeams[3].GoalDifference);
        rankedTeams[2].GoalsFor.Should().Be(rankedTeams[3].GoalsFor);
        rankedTeams[2].GoalsAgainst.Should().Be(rankedTeams[3].GoalsAgainst);
    }
}