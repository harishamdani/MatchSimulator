namespace TournamentSimulator.Core.Entities.Results;

public class TournamentSimulationResult
{
    public List<RoundResult> Rounds { get; set; } = [];
    public List<TeamResult> FinalResults { get; set; } = [];
}

public class RoundResult
{
    public int RoundNumber { get; set; }
    public List<MatchResult> Matches { get; set; } = [];
    public string RestingTeam { get; set; }
}

public class MatchResult
{
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
}

public class TeamResult
{
    public string Name { get; set; }
    public int Ranking { get; set; }
    public int Points { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int GoalDifference { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
}