namespace TournamentSimulator.Core.Entities;

public class Team(string name, int strength)
{
    public string Name { get; set; } = name;
    public int Strength { get; set; } = strength;
    public int Points { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int Ranking { get; set; }
}