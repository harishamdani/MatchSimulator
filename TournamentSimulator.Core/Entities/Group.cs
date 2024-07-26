namespace TournamentSimulator.Core.Entities;

public class Group
{
    public List<Team> Teams { get; set; } = [];
    public List<Match> Matches { get; set; } = [];

    public Group(){}

    public Group(List<Team> teams, List<Match> matches)
    {
        Teams = teams; 
        Matches = matches;
    }
}