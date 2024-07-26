using FluentAssertions;
using Moq;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;
using TournamentSimulator.Core.Services;
using Match = TournamentSimulator.Core.Entities.Match;

namespace TournamentSimulator.UnitTests.Services;

[TestFixture]
public class GroupSimulatorTests
{
    private Mock<IMatchSimulator> _mockMatchSimulator;
    private GroupSimulator _groupSimulator;

    [SetUp]
    public void Setup()
    {
        _mockMatchSimulator = new Mock<IMatchSimulator>();
        _groupSimulator = new GroupSimulator(_mockMatchSimulator.Object);
    }

    [Test]
    public void SimulateGroup_TeamsWithDifferentPoints_ShouldSortCorrectly()
    {
        var teamA = new Team("Team A", 50);
        var teamB = new Team("Team B", 50);
        var teamC = new Team("Team C", 50);

        var group = new Group([teamA, teamB, teamC], []);

        var simulatedMatches = new List<Match>
        {
            new Match(teamA, teamB, 2, 0, 1),
            new Match(teamA, teamC, 1, 0, 2),
            new Match(teamB, teamC, 0, 0, 3)
        };

        _mockMatchSimulator
            .Setup(m => m.SimulatedMatches(group))
            .Returns(simulatedMatches);

        var simulatedGroup = _groupSimulator.SimulateGroup(group);

        simulatedGroup.Teams[0].Should().Be(teamA);
        simulatedGroup.Teams[1].Should().Be(teamC);
        simulatedGroup.Teams[2].Should().Be(teamB);

        simulatedGroup.Teams[0].Ranking.Should().Be(1);
        simulatedGroup.Teams[1].Ranking.Should().Be(2);
        simulatedGroup.Teams[2].Ranking.Should().Be(3);

        simulatedGroup.Teams[0].Points.Should().Be(6);
        simulatedGroup.Teams[1].Points.Should().Be(1);
        simulatedGroup.Teams[2].Points.Should().Be(1);
    }

    [Test]
    public void SimulateGroup_TeamsWithSamePoints_ShouldSortByGoalDifference()
    {
        var teamA = new Team("Team A", 50);
        var teamB = new Team("Team B", 50);
        var teamC = new Team("Team C", 50);

        var group = new Group([teamA, teamB, teamC], []);

        var simulatedMatches = new List<Match>
        {
            new Match(teamA, teamB, 2, 0, 1),
            new Match(teamA, teamC, 0, 2, 2),
            new Match(teamB, teamC, 1, 0, 3)
        };

        _mockMatchSimulator
            .Setup(m => m.SimulatedMatches(group))
            .Returns(simulatedMatches);

        var simulatedGroup = _groupSimulator.SimulateGroup(group);

        simulatedGroup.Teams[0].Should().Be(teamC);
        simulatedGroup.Teams[1].Should().Be(teamA);
        simulatedGroup.Teams[2].Should().Be(teamB);

        simulatedGroup.Teams[0].Ranking.Should().Be(1);
        simulatedGroup.Teams[1].Ranking.Should().Be(2);
        simulatedGroup.Teams[2].Ranking.Should().Be(3);

        simulatedGroup.Teams[0].Points.Should().Be(3);
        simulatedGroup.Teams[1].Points.Should().Be(3);
        simulatedGroup.Teams[2].Points.Should().Be(3);

        simulatedGroup.Teams[0].GoalDifference.Should().Be(1);
        simulatedGroup.Teams[1].GoalDifference.Should().Be(0);
        simulatedGroup.Teams[2].GoalDifference.Should().Be(-1);
    }

    [Test]
    public void SimulateGroup_TeamsWithSamePointsAndGoalDifference_ShouldSortByGoalsFor()
    {
        var teamA = new Team("Team A", 50);
        var teamB = new Team("Team B", 50);
        var teamC = new Team("Team C", 50);

        var group = new Group([teamA, teamB, teamC], []);

        var simulatedMatches = new List<Match>
        {
            new Match(teamA, teamB, 2, 1, 1),
            new Match(teamA, teamC, 1, 2, 2),
            new Match(teamB, teamC, 3, 2, 3)
        };

        _mockMatchSimulator
            .Setup(m => m.SimulatedMatches(group))
            .Returns(simulatedMatches);

        var simulatedGroup = _groupSimulator.SimulateGroup(group);

        simulatedGroup.Teams[0].Should().Be(teamB);
        simulatedGroup.Teams[1].Should().Be(teamC);
        simulatedGroup.Teams[2].Should().Be(teamA);

        simulatedGroup.Teams[0].Ranking.Should().Be(1);
        simulatedGroup.Teams[1].Ranking.Should().Be(2);
        simulatedGroup.Teams[2].Ranking.Should().Be(3);

        simulatedGroup.Teams[0].Points.Should().Be(3);
        simulatedGroup.Teams[1].Points.Should().Be(3);
        simulatedGroup.Teams[2].Points.Should().Be(3);

        simulatedGroup.Teams[0].GoalDifference.Should().Be(0);
        simulatedGroup.Teams[1].GoalDifference.Should().Be(0);
        simulatedGroup.Teams[2].GoalDifference.Should().Be(0);

        simulatedGroup.Teams[0].GoalsFor.Should().Be(4);
        simulatedGroup.Teams[1].GoalsFor.Should().Be(4);
        simulatedGroup.Teams[2].GoalsFor.Should().Be(3);
    }

    [Test]
    public void SimulateGroup_TeamsWithSamePointsGoalDifferenceAndGoalsFor_ShouldSortByGoalsAgainst()
    {
        var teamA = new Team("Team A", 50);
        var teamB = new Team("Team B", 50);
        var teamC = new Team("Team C", 50);

        var group = new Group([teamA, teamB, teamC], []);

        var simulatedMatches = new List<Match>
        {
            new(teamA, teamB, 2, 2, 1),
            new(teamA, teamC, 2, 1, 2),
            new(teamB, teamC, 2, 3, 3)
        };

        _mockMatchSimulator
            .Setup(m => m.SimulatedMatches(group))
            .Returns(simulatedMatches);

        var simulatedGroup = _groupSimulator.SimulateGroup(group);

        simulatedGroup.Teams[0].Should().Be(teamA);
        simulatedGroup.Teams[1].Should().Be(teamC);
        simulatedGroup.Teams[2].Should().Be(teamB);

        simulatedGroup.Teams[0].Ranking.Should().Be(1);
        simulatedGroup.Teams[1].Ranking.Should().Be(2);
        simulatedGroup.Teams[2].Ranking.Should().Be(3);

        simulatedGroup.Teams[0].Points.Should().Be(4);
        simulatedGroup.Teams[1].Points.Should().Be(3);
        simulatedGroup.Teams[2].Points.Should().Be(1);

        simulatedGroup.Teams[0].GoalDifference.Should().Be(1);
        simulatedGroup.Teams[1].GoalDifference.Should().Be(0);
        simulatedGroup.Teams[2].GoalDifference.Should().Be(-1);

        simulatedGroup.Teams[0].GoalsFor.Should().Be(4);
        simulatedGroup.Teams[1].GoalsFor.Should().Be(4);
        simulatedGroup.Teams[2].GoalsFor.Should().Be(4);

        simulatedGroup.Teams[0].GoalsAgainst.Should().Be(3);
        simulatedGroup.Teams[1].GoalsAgainst.Should().Be(4);
        simulatedGroup.Teams[2].GoalsAgainst.Should().Be(5);
    }
    
    [Test]
    public void SimulateGroup_TeamsWithSameStatsDrawHeadToHead_ShouldMaintainOriginalOrder()
    {
        var teamA = new Team("Team A", 50);
        var teamB = new Team("Team B", 50);
        var teamC = new Team("Team C", 50);

        var group = new Group(new List<Team> { teamA, teamB, teamC }, new List<Match>());

        var simulatedMatches = new List<Match>
        {
            new Match(teamA,
                teamB,
                1,
                1,
                1
            ),
            new Match(teamB,
                teamC,
                1,
                1,
                2
            ),
            new Match(teamC,
                teamA,
                1,
                1,
                3
            )
        };

        _mockMatchSimulator
            .Setup(m => m.SimulatedMatches(group))
            .Returns(simulatedMatches);

        var simulatedGroup = _groupSimulator.SimulateGroup(group);

        simulatedGroup
            .Teams[0]
            .Should()
            .Be(teamA);
        simulatedGroup
            .Teams[1]
            .Should()
            .Be(teamB);
        simulatedGroup
            .Teams[2]
            .Should()
            .Be(teamC);

        simulatedGroup
            .Teams[0]
            .Ranking
            .Should()
            .Be(1);
        simulatedGroup
            .Teams[1]
            .Ranking
            .Should()
            .Be(2);
        simulatedGroup
            .Teams[2]
            .Ranking
            .Should()
            .Be(3);

        simulatedGroup
            .Teams[0]
            .Points
            .Should()
            .Be(2);
        simulatedGroup
            .Teams[1]
            .Points
            .Should()
            .Be(2);
        simulatedGroup
            .Teams[2]
            .Points
            .Should()
            .Be(2);

        simulatedGroup
            .Teams[0]
            .GoalDifference
            .Should()
            .Be(0);
        simulatedGroup
            .Teams[1]
            .GoalDifference
            .Should()
            .Be(0);
        simulatedGroup
            .Teams[2]
            .GoalDifference
            .Should()
            .Be(0);

        simulatedGroup
            .Teams[0]
            .GoalsFor
            .Should()
            .Be(2);
        simulatedGroup
            .Teams[1]
            .GoalsFor
            .Should()
            .Be(2);
        simulatedGroup
            .Teams[2]
            .GoalsFor
            .Should()
            .Be(2);
    }
    
    [Test]
    public void SimulateGroup_AllTeamsWithZeroPoints_ShouldSortAlphabetically()
    {
        var teamA = new Team("Team A", 50);
        var teamB = new Team("Team B", 50);
        var teamC = new Team("Team C", 50);

        var group = new Group([teamA, teamB, teamC], []);

        var simulatedMatches = new List<Match>
        {
            new Match(teamA, teamB, 0, 0, 1),
            new Match(teamB, teamC, 0, 0, 2),
            new Match(teamC, teamA, 0, 0, 3)
        };

        _mockMatchSimulator.Setup(m => m.SimulatedMatches(group)).Returns(simulatedMatches);

        var simulatedGroup = _groupSimulator.SimulateGroup(group);

        simulatedGroup.Teams[0].Should().Be(teamA);
        simulatedGroup.Teams[1].Should().Be(teamB);
        simulatedGroup.Teams[2].Should().Be(teamC);

        simulatedGroup.Teams[0].Ranking.Should().Be(1);
        simulatedGroup.Teams[1].Ranking.Should().Be(2);
        simulatedGroup.Teams[2].Ranking.Should().Be(3);

        simulatedGroup.Teams[0].Points.Should().Be(2);
        simulatedGroup.Teams[1].Points.Should().Be(2);
        simulatedGroup.Teams[2].Points.Should().Be(2);

        simulatedGroup.Teams[0].GoalDifference.Should().Be(0);
        simulatedGroup.Teams[1].GoalDifference.Should().Be(0);
        simulatedGroup.Teams[2].GoalDifference.Should().Be(0);

        simulatedGroup.Teams[0].GoalsFor.Should().Be(0);
        simulatedGroup.Teams[1].GoalsFor.Should().Be(0);
        simulatedGroup.Teams[2].GoalsFor.Should().Be(0);
    }
    
}