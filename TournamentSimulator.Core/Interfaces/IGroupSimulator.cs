using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Interfaces;

public interface IGroupSimulator
{
    Group SimulateGroup(Group group);
}