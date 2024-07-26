using FluentAssertions;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Services;

namespace TournamentSimulator.UnitTests.Services
{
    [TestFixture]
    public class MatchSimulatorTests
    {
        private MatchSimulator _matchSimulator;

        [SetUp]
        public void Setup()
        {
            _matchSimulator = new MatchSimulator();
        }

        [Test]
        public void SimulateMatch_ShouldGenerateGoalsWithinExpectedRange()
        {
            var homeTeam = new Team("Home", 50);
            var awayTeam = new Team("Away", 50);

            var match = _matchSimulator.SimulateMatch(homeTeam, awayTeam);

            match.HomeGoals.Should().BeInRange(0, 5);
            match.AwayGoals.Should().BeInRange(0, 5);
        }

        [Test]
        public void SimulatedMatches_ShouldSimulateAllMatchesInGroup()
        {
            var teamA = new Team("Team A", 50);
            var teamB = new Team("Team B", 50);
            var teamC = new Team("Team C", 50);
            var group = new Group([teamA, teamB, teamC],
                [new(teamA, teamB), new (teamA, teamC), new (teamB, teamC)]
            );

            var simulatedMatches = _matchSimulator.SimulatedMatches(group);

            simulatedMatches.Should().HaveCount(3);
            simulatedMatches.Should().OnlyContain(m => m.HomeGoals >= 0 && m.AwayGoals >= 0);
        }
    }
}