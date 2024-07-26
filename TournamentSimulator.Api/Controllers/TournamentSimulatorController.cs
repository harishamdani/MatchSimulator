using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TournamentSimulator.Core.Interfaces;

[ApiController]
[Route("[controller]")]
public class TournamentSimulatorController(ITournamentSimulatorService service) : ControllerBase
{
    [HttpGet("simulate/{numberOfTeams}")]
    public IActionResult SimulateTournament(int numberOfTeams)
    {
        if (numberOfTeams < 2)
        {
            return BadRequest("Number of teams must be at least 2.");
        }

        try
        {
            var result = service.SimulateGroup(numberOfTeams);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
        }
    }
}