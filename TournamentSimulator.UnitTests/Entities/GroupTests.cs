using FluentAssertions;
using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.UnitTests.Entities;

[TestFixture]
public class GroupTests
{
    [Test]
    public void Constructor_WithEmptyLists_ShouldCreateEmptyGroup()
    {
        var group = new Group();

        group.Teams.Should().BeEmpty();
        group.Matches.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WithNonEmptyLists_ShouldCreateGroupWithTeamsAndMatches()
    {
        var teams = new List<Team> { new("Team A", 50), new("Team B", 50) };
        var matches = new List<Match> { new(teams[0], teams[1]) };

        var group = new Group(teams, matches);

        group.Teams.Should().BeEquivalentTo(teams);
        group.Matches.Should().BeEquivalentTo(matches);
    }
}