
using Castle.Components.DictionaryAdapter;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;
using TournamentSimulator.Core.Services;

namespace TournamentSimulator.UnitTests.Services;

[TestFixture]
public class TournamentSimulatorServiceTests
{
    private Mock<ITeamGenerator> _mockTeamGenerator;
    private Mock<IGroupGenerator> _mockGroupGenerator;
    private Mock<IGroupSimulator> _mockGroupSimulator;
    private Mock<IValidator<Group>> _mockGroupValidator;
    private Mock<IValidator<Team>> _mockTeamValidator;
    private TournamentSimulatorService _service;

    [SetUp]
    public void Setup()
    {
        _mockTeamGenerator = new Mock<ITeamGenerator>();
        _mockGroupGenerator = new Mock<IGroupGenerator>();
        _mockGroupSimulator = new Mock<IGroupSimulator>();
        _mockGroupValidator = new Mock<IValidator<Group>>();
        _mockTeamValidator = new Mock<IValidator<Team>>();

        _service = new TournamentSimulatorService(
            _mockTeamGenerator.Object,
            _mockGroupGenerator.Object,
            _mockGroupSimulator.Object,
            _mockGroupValidator.Object,
            _mockTeamValidator.Object
        );
    }
    
    [Test]
    public void SimulateGroup_InvalidTeam_ThrowsValidationException()
    {
        // Arrange
        int numberOfTeams = 2;
        _mockTeamGenerator
            .Setup(x => x.GenerateTeams(numberOfTeams))
            .Returns(new EditableList<Team>()
                {
                    new Team(string.Empty, 25),
                    new Team(string.Empty, 25),
                }
            );
        _mockTeamValidator.Setup(v => v.Validate(It.IsAny<Team>()))
            .Returns(new ValidationResult(new[] { new ValidationFailure("Name", "Team name is required") }));

        // Act & Assert
        Assert.Throws<ValidationException>(() => _service.SimulateGroup(numberOfTeams));
    }

    [Test]
    public void SimulateGroup_InvalidGroup_ThrowsValidationException()
    {
        // Arrange
        int numberOfTeams = 2;
        _mockTeamGenerator
            .Setup(x => x.GenerateTeams(numberOfTeams))
            .Returns(new EditableList<Team>()
                {
                    new Team("Team A", 25),
                    new Team("Team B", 25),
                }
            );
        _mockTeamValidator.Setup(v => v.Validate(It.IsAny<Team>())).Returns(new ValidationResult());
        _mockGroupValidator.Setup(v => v.Validate(It.IsAny<Group>()))
            .Returns(new ValidationResult(new[] { new ValidationFailure("Teams", "A group must have at least two teams") }));

        // Act & Assert
        Assert.Throws<ValidationException>(() => _service.SimulateGroup(numberOfTeams));
    }
}