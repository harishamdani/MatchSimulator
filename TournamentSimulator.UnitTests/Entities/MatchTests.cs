using FluentAssertions;
using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.UnitTests.Entities;

[TestFixture]
public class MatchTests
{
    [Test]
    public void Constructor_WithAllParameters_ShouldCreateMatchCorrectly()
    {
        var homeTeam = new Team("Home", 50);
        var awayTeam = new Team("Away", 50);
        var match = new Match(homeTeam, awayTeam, 2, 1, 1);

        match.HomeTeam.Should().Be(homeTeam);
        match.AwayTeam.Should().Be(awayTeam);
        match.HomeGoals.Should().Be(2);
        match.AwayGoals.Should().Be(1);
        match.Round.Should().Be(1);
    }

    [Test]
    public void Constructor_WithOnlyTeams_ShouldCreateMatchWithDefaultValues()
    {
        var homeTeam = new Team("Home", 50);
        var awayTeam = new Team("Away", 50);
        var match = new Match(homeTeam, awayTeam);

        match.HomeTeam.Should().Be(homeTeam);
        match.AwayTeam.Should().Be(awayTeam);
        match.HomeGoals.Should().Be(0);
        match.AwayGoals.Should().Be(0);
        match.Round.Should().Be(0);
    }
}