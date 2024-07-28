using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TournamentSimulator.Core.Interfaces;

[ApiController]
[Route("[controller]")]
public class TournamentSimulatorController(ITournamentSimulatorService service) : ControllerBase
{
    [HttpGet("simulate/{numberOfTeams}/{numberOfQualifiedTeams}")]
    public IActionResult SimulateTournament(int numberOfTeams, int numberOfQualifiedTeams)
    {
        if (numberOfTeams < 2)
        {
            return BadRequest("Number of teams must be at least 2.");
        }
        
        if (numberOfQualifiedTeams < 1 || numberOfQualifiedTeams >= numberOfTeams)
        {
            return BadRequest("Number of qualified teams must be smaller than the number of teams");
        }

        try
        {
            var result = service.SimulateGroup(numberOfTeams, numberOfQualifiedTeams);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
        }
    }
}