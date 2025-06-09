using APBD_TEST_LastTest.Data;
using APBD_TEST_LastTest.Exceptions;
using APBD_TEST_LastTest.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TEST_LastTest.Controller;
[ApiController]
[Route("api/track-races")]
public class TrackRacesController : ControllerBase
{
    private IDbService _dbService;

    public TrackRacesController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost("participants")]
    public async Task<IActionResult> AddParticipations(AddParticipationsDTO addParticipationsDto)
    {
        try
        {

            if (addParticipationsDto.Participations.Count == 0)
            {
                return BadRequest("Participations cannot be empty");
            }
            
            await _dbService.AddParticipations(addParticipationsDto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }
}