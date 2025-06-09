using APBD_TEST_LastTest.Data;
using APBD_TEST_LastTest.DTOs;
using APBD_TEST_LastTest.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_TEST_LastTest.Service;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<GetRacesDTO> GetParticipations(int racerId)
    {
        // var racer = await _context.Racers.FirstOrDefaultAsync(r => r.RacerId == racerId);
        // if (racer == null)
        // {
        //     throw new NotFoundException("Racer not found");
        // }

        var participations = await _context.Racers
            .Where(r => r.RacerId == racerId)
            .Select(r =>
                new GetRacesDTO()
                {
                    RacerId = r.RacerId,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Participations = r.RaceParticipations.Select(
                        p => new GetParticipationDTO()
                        {
                            Race = new RaceDTO()
                            {
                                Name = p.TrackRace.Race.Name,
                                Location = p.TrackRace.Race.Location,
                                Date = p.TrackRace.Race.Date,
                            },
                            Track = new TrackDTO()
                            {
                                Name = p.TrackRace.Track.Name,
                                LengthInKM = p.TrackRace.Track.LengthInKM,
                            },
                            laps = p.TrackRace.Laps,
                            Position = p.Position,
                            FinishTimeInSeconds = p.FinishTimeInSeconds
                        }
                    ).ToList()
                }
            ).FirstOrDefaultAsync();

        if (participations == null)
        {
            throw new NotFoundException("Participations not found");
        }
        
        return participations;
    }
}