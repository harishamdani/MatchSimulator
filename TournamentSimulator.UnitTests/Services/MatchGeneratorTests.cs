using FluentAssertions;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;
using TournamentSimulator.Core.Services;

namespace TournamentSimulator.UnitTests.Services;

[TestFixture]
public class MatchGeneratorTests
{
    private IMatchGenerator _matchGenerator;

    [SetUp]
    public void Setup()
    {
        _matchGenerator = new MatchGenerator();
    }

    [Test]
    public void GenerateMatches_WithTwoTeams_ShouldGenerateOneMatch()
    {
        var teams = new List<Team>
        {
            new Team("Team A", 50),
            new Team("Team B", 50)
        };

        var matches = _matchGenerator.GenerateMatches(teams);

        matches.Should().HaveCount(1);
        matches[0].HomeTeam.Should().Be(teams[0]);
        matches[0].AwayTeam.Should().Be(teams[1]);
        matches[0].Round.Should().Be(1);
    }

    [Test]
    public void GenerateMatches_WithThreeTeams_ShouldGenerateThreeMatches()
    {
        var teams = new List<Team>
        {
            new Team("Team A", 50),
            new Team("Team B", 50),
            new Team("Team C", 50)
        };

        var matches = _matchGenerator.GenerateMatches(teams);

        matches.Should().HaveCount(3);
        matches.Select(m => m.Round).Distinct().Should().BeEquivalentTo(new[] { 1, 2, 3 });
        teams.All(t => matches.Count(m => m.HomeTeam == t || m.AwayTeam == t) == 2).Should().BeTrue();

        // Each round should have exactly one match
        matches.GroupBy(m => m.Round).All(g => g.Count() == 1).Should().BeTrue();

        // Each team should play against each other team once
        matches.Select(m => new { Home = m.HomeTeam, Away = m.AwayTeam })
            .Select(m => new HashSet<Team> { m.Home, m.Away })
            .Distinct(HashSet<Team>.CreateSetComparer())
            .Should().HaveCount(3);
    }

    [Test]
    public void GenerateMatches_WithFourTeams_ShouldGenerateSixMatches()
    {
        var teams = new List<Team>
        {
            new Team("Team A", 50),
            new Team("Team B", 50),
            new Team("Team C", 50),
            new Team("Team D", 50)
        };

        var matches = _matchGenerator.GenerateMatches(teams);

        matches.Should().HaveCount(6);
        matches.Select(m => m.Round).Distinct().Should().BeEquivalentTo(new[] { 1, 2, 3 });
        teams.All(t => matches.Count(m => m.HomeTeam == t || m.AwayTeam == t) == 3).Should().BeTrue();
    }

    [Test]
    public void GenerateMatches_WithFiveTeams_ShouldGenerateTenMatches()
    {
        var teams = new List<Team>
        {
            new Team("Team A", 50),
            new Team("Team B", 50),
            new Team("Team C", 50),
            new Team("Team D", 50),
            new Team("Team E", 50)
        };

        var matches = _matchGenerator.GenerateMatches(teams);

        matches.Should().HaveCount(10);
        matches.Select(m => m.Round).Distinct().Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        teams.All(t => matches.Count(m => m.HomeTeam == t || m.AwayTeam == t) == 4).Should().BeTrue();
    }

    [Test]
    public void GenerateMatches_ShouldEnsureEachTeamPlaysOncePerRound()
    {
        var teams = new List<Team>
        {
            new Team("Team A", 50),
            new Team("Team B", 50),
            new Team("Team C", 50),
            new Team("Team D", 50),
            new Team("Team E", 50)
        };

        var matches = _matchGenerator.GenerateMatches(teams);

        foreach (var round in matches.Select(m => m.Round).Distinct())
        {
            var teamsInRound = matches.Where(m => m.Round == round)
                .SelectMany(m => new[] { m.HomeTeam, m.AwayTeam })
                .ToList();
            teamsInRound.Should().OnlyHaveUniqueItems();
            teamsInRound.Should().HaveCount(teams.Count % 2 == 0 ? teams.Count : teams.Count - 1);
        }
    }

    [Test]
    public void GenerateMatches_WithOneTeam_ShouldReturnEmptyList()
    {
        var teams = new List<Team> { new Team("Team A", 50) };

        var matches = _matchGenerator.GenerateMatches(teams);

        matches.Should().BeEmpty();
    }

    [Test]
    public void GenerateMatches_WithEmptyList_ShouldReturnEmptyList()
    {
        var teams = new List<Team>();

        var matches = _matchGenerator.GenerateMatches(teams);

        matches.Should().BeEmpty();
    }

    [Test]
    public void GenerateMatches_ShouldNotModifyOriginalTeamList()
    {
        var teams = new List<Team>
        {
            new Team("Team A", 50),
            new Team("Team B", 50),
            new Team("Team C", 50)
        };
        var originalTeams = new List<Team>(teams);

        _matchGenerator.GenerateMatches(teams);

        teams.Should().BeEquivalentTo(originalTeams);
    }
}