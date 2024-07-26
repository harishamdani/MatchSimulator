// using FluentAssertions;
// using Moq;
// using TournamentSimulator.Core.Entities;
// using TournamentSimulator.Core.Interfaces;
// using TournamentSimulator.Core.Services;
//
// namespace TournamentSimulator.UnitTests.Services;
//
// [TestFixture]
// public class GroupGeneratorTests
// {
//     private GroupGenerator _groupGenerator;
//     private Mock<IMatchGenerator> _mockMatchGenerator;
//
//     [SetUp]
//     public void Setup()
//     {
//         _mockMatchGenerator = new Mock<IMatchGenerator>();
//         _groupGenerator = new GroupGenerator(_mockMatchGenerator.Object);
//     }
//
//     [Test]
//     public void GenerateGroup_WithTwoTeams_ShouldGenerateOneMatch()
//     {
//         var teams = new List<Team>
//         {
//             new ("Team A", 50),
//             new ("Team B", 50)
//         };
//
//         _mockMatchGenerator.Setup(x => x.GenerateMatches(It.IsAny<List<Team>>())).Returns<List<Team>>((teams) => )
//         var group = _groupGenerator.GenerateGroup(teams);
//
//         group.Teams.Should().BeEquivalentTo(teams);
//         group.Matches.Should().HaveCount(1);
//         group.Matches[0].HomeTeam.Should().Be(teams[0]);
//         group.Matches[0].AwayTeam.Should().Be(teams[1]);
//     }
//
//     [Test]
//     public void GenerateGroup_WithFourTeams_ShouldGenerateSixMatches()
//     {
//         var teams = new List<Team>
//         {
//             new ("Team A", 50),
//             new ("Team B", 50),
//             new ("Team C", 50),
//             new ("Team D", 50)
//         };
//
//         var group = _groupGenerator.GenerateGroup(teams);
//
//         group.Teams.Should().BeEquivalentTo(teams);
//         group.Matches.Should().HaveCount(6);
//     }
//
//     [Test]
//     public void GenerateGroup_WithLessThanTwoTeams_ShouldThrowArgumentException()
//     {
//         var teams = new List<Team> { new ("Team A", 50) };
//
//         Action act = () => _groupGenerator.GenerateGroup(teams);
//
//         act.Should().Throw<ArgumentException>()
//             .WithMessage("At least two teams are required to generate a group.");
//     }
// }