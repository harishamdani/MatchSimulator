using FluentAssertions;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Validators;

namespace TournamentSimulator.UnitTests.Validators;

[TestFixture]
public class GroupValidatorTests
{
    private GroupValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GroupValidator();
    }

    [Test]
    public void Validate_WithValidGroup_ShouldPassValidation()
    {
        var group = new Group
        {
            Teams = [new("Team A", 50)],
            Matches = []
        };

        var result = _validator.Validate(group);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithEmptyTeamList_ShouldFailValidation()
    {
        var group = new Group
        {
            Teams = [],
            Matches = []
        };

        var result = _validator.Validate(group);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Teams" && e.ErrorMessage.Contains("must have at least one team"));
    }
}