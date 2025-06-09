using APBD_TEST_LastTest.Exceptions;
using APBD_TEST_LastTest.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TEST_LastTest.Controller;

[ApiController]
[Route("api/racers")]
public class RacersController : ControllerBase
{
    private readonly IDbService _dbService;

    public RacersController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}/participations")]
    public async Task<IActionResult> GetParticipations(int id)
    {
        try
        {
            var participations = await _dbService.GetParticipations(id);
            return Ok(participations);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    
}