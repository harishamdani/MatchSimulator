using FluentValidation;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Entities.Results;
using TournamentSimulator.Core.Interfaces;

namespace TournamentSimulator.Core.Services;

public class TournamentSimulatorService(
    ITeamGenerator teamGenerator,
    IGroupGenerator groupGenerator,
    IGroupSimulator groupSimulator,
    IValidator<Group> groupValidator,
    IValidator<Team> teamValidator)
    : ITournamentSimulatorService
{
    public TournamentSimulationResult SimulateGroup(int numberOfTeams)
    {
        var teams = teamGenerator.GenerateTeams(numberOfTeams);
        ValidateTeams(teams);

        var group = groupGenerator.GenerateGroup(teams);
        ValidateGroups(group);

        var simulatedGroup = groupSimulator.SimulateGroup(group);

        return CreateSimulationResult(simulatedGroup);
    }

    private void ValidateGroups(Group group)
    {
        // Validate the group
        var groupValidationResult = groupValidator.Validate(group);
        if (!groupValidationResult.IsValid)
        {
            throw new ValidationException(groupValidationResult.Errors);
        }
    }

    private void ValidateTeams(List<Team> teams)
    {
        // Validate each team
        foreach (var teamValidationResult in teams.Select(teamValidator.Validate).Where(teamValidationResult => !teamValidationResult.IsValid))
        {
            throw new ValidationException(teamValidationResult.Errors);
        }
    }

    private static TournamentSimulationResult CreateSimulationResult(Group simulatedGroup)
    {
        var result = new TournamentSimulationResult();

        var matchesByRound = simulatedGroup.Matches
            .Select((match, index) => (match, round: index / (simulatedGroup.Teams.Count / 2) + 1))
            .GroupBy(x => x.round);

        result.Rounds = CreateRoundResults(simulatedGroup, matchesByRound);

        result.FinalResults = simulatedGroup.Teams
            .Select(team => new TeamResult
            {
                Name = team.Name,
                Ranking = team.Ranking,
                Points = team.Points,
                Wins = team.Wins,
                Draws = team.Draws,
                Losses = team.Losses,
                GoalDifference = team.GoalDifference,
                GoalsFor = team.GoalsFor,
                GoalsAgainst = team.GoalsAgainst
            }).ToList();

        return result;
    }

    private static List<RoundResult> CreateRoundResults(Group simulatedGroup, IEnumerable<IGrouping<int, (Match match, int round)>> matchesByRound)
    {
        var rounds = new List<RoundResult>();
        foreach (var round in matchesByRound)
        {
            var roundResult = new RoundResult { RoundNumber = round.Key };

            foreach (var (match, _) in round)
            {
                roundResult.Matches.Add(new MatchResult
                {
                    HomeTeam = match.HomeTeam.Name,
                    AwayTeam = match.AwayTeam.Name,
                    HomeGoals = match.HomeGoals,
                    AwayGoals = match.AwayGoals
                });
            }

            var restingTeam = simulatedGroup.Teams.FirstOrDefault(team =>
                !round.Any(m => m.match.HomeTeam.Name == team.Name || m.match.AwayTeam.Name == team.Name));

            roundResult.RestingTeam = restingTeam?.Name ?? "None";

            rounds.Add(roundResult);
        }

        return rounds;
    }
}