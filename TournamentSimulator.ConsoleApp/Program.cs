using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Services;
using TournamentSimulator.Core.Validators;

var teams =new List<Team>{
    new("Team A", 80),
    new("Team B", 75),
    new("Team C", 70),
    new("Team D", 65),
    new("Team E", 60)
};

var matchGenerator = new MatchGenerator();
var groupGenerator = new GroupGenerator(matchGenerator);
var initialGroup = groupGenerator.GenerateGroup(teams);

var groupValidator = new GroupValidator();
var validationResult = groupValidator.Validate(initialGroup);

if (!validationResult.IsValid)
{
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"Validation error: {error}");
    }
    return;
}

var matchSimulator = new MatchSimulator();
var groupSimulator = new GroupSimulator(matchSimulator);

var simulatedGroup = groupSimulator.SimulateGroup(initialGroup);

Console.WriteLine("Matches by Round:");
Console.WriteLine("------------------");
var matchesByRound = simulatedGroup.Matches
    .Select((match, index) => (match, round: index / (teams.Count / 2) + 1))
    .GroupBy(x => x.round);

foreach (var round in matchesByRound)
{
    Console.WriteLine($"\nRound {round.Key}:");
    foreach (var (match, _) in round)
    {
        Console.WriteLine($"{match.HomeTeam.Name} vs {match.AwayTeam.Name}: {match.HomeGoals} - {match.AwayGoals}");
    }
    var restingTeam = teams.First(team => !round.Any(m => m.match.HomeTeam.Name == team.Name || m.match.AwayTeam.Name == team.Name));
    Console.WriteLine($"Resting: {restingTeam.Name}");
}

Console.WriteLine("\nFinal Group Stage Results:");
Console.WriteLine("---------------------------");

foreach (var team in simulatedGroup.Teams)
{
    Console.WriteLine($"{team.Name}: Points: {team.Points}, W: {team.Wins}, D: {team.Draws}, L: {team.Losses}, GD: {team.GoalDifference}, GF: {team.GoalsFor}, GA: {team.GoalsAgainst}");
}

Console.WriteLine("\nTeams advancing to the knockout stage:");
Console.WriteLine("---------------------------------------");
Console.WriteLine($"1. {simulatedGroup.Teams[0].Name}");
Console.WriteLine($"2. {simulatedGroup.Teams[1].Name}");