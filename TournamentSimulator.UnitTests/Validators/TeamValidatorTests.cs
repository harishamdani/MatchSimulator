using FluentAssertions;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Validators;

namespace TournamentSimulator.UnitTests.Validators;

[TestFixture]
public class TeamValidatorTests
{
    private TeamValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TeamValidator();
    }

    [Test]
    public void Validate_WithValidTeam_ShouldPassValidation()
    {
        var team = new Team("Valid Team", 50);

        var result = _validator.Validate(team);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithEmptyName_ShouldFailValidation()
    {
        var team = new Team("", 50);

        var result = _validator.Validate(team);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WithNameTooLong_ShouldFailValidation()
    {
        var team = new Team(new string('A', 51), 50);

        var result = _validator.Validate(team);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Name" && 
            e.ErrorMessage.Contains("must be 50 characters or fewer. "));
    }

    [Test]
    public void Validate_WithStrengthOutOfRange_ShouldFailValidation()
    {
        var teamTooWeak = new Team("Weak Team", 0);
        var teamTooStrong = new Team("Strong Team", 101);

        var weakResult = _validator.Validate(teamTooWeak);
        var strongResult = _validator.Validate(teamTooStrong);

        weakResult.IsValid.Should().BeFalse();
        strongResult.IsValid.Should().BeFalse();
        weakResult.Errors.Should().Contain(e => e.PropertyName == "Strength" && e.ErrorMessage.Contains("between 1 and 100"));
        strongResult.Errors.Should().Contain(e => e.PropertyName == "Strength" && e.ErrorMessage.Contains("between 1 and 100"));
    }
}