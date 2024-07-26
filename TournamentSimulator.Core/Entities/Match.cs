namespace TournamentSimulator.Core.Entities;

public class Match(
    Team homeTeam,
    Team awayTeam,
    int homeGoals,
    int awayGoals,
    int roundNumber)
{
    public Team HomeTeam { get; set; } = homeTeam;
    public Team AwayTeam { get; set; } = awayTeam;
    public int HomeGoals { get; set; } = homeGoals;
    public int AwayGoals { get; set; } = awayGoals;
    public int Round { get; set; } = roundNumber;

    public Match(Team homeTeam, Team awayTeam) : this(homeTeam, awayTeam,
        0,
        0,
        0
    )
    {
    }
}