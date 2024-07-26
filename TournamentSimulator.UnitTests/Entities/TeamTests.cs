using FluentAssertions;
using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.UnitTests.Entities;

[TestFixture]
public class TeamTests
{
    [Test]
    public void Constructor_ShouldCreateTeamCorrectly()
    {
        var team = new Team("Test Team", 75);

        team.Name.Should().Be("Test Team");
        team.Strength.Should().Be(75);
        team.Points.Should().Be(0);
        team.GoalsFor.Should().Be(0);
        team.GoalsAgainst.Should().Be(0);
        team.Wins.Should().Be(0);
        team.Draws.Should().Be(0);
        team.Losses.Should().Be(0);
    }

    [Test]
    public void GoalDifference_ShouldCalculateCorrectly()
    {
        var team = new Team("Test Team", 75)
        {
            GoalsFor = 10,
            GoalsAgainst = 5
        };

        team.GoalDifference.Should().Be(5);
    }
}