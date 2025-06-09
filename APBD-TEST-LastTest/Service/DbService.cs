using APBD_TEST_LastTest.Data;
using APBD_TEST_LastTest.DTOs;
using APBD_TEST_LastTest.Exceptions;
using APBD_TEST_LastTest.Models;
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

    public async Task AddParticipations(AddParticipationsDTO addParticipationsDto)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var race = await _context.Races.FirstOrDefaultAsync(r => r.Name == addParticipationsDto.RaceName);
            if (race == null)
            {
                throw new NotFoundException("Race not found");
            }
            
            var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Name == addParticipationsDto.TrackName);
            if (track == null)
            {
                throw new NotFoundException("Track not found");
            }
            
            var trackRace = await _context.TrackRaces.FirstOrDefaultAsync(tr => tr.TrackRaceId == track.TrackId && tr.RaceId == race.RaceId);
            if (trackRace == null)
            {
                throw new NotFoundException("Race on this track not found");
            }

            foreach (var part in addParticipationsDto.Participations)
            {
                if (part.FinishTimeInSeconds < trackRace.BestTimeInSeconds)
                {
                    trackRace.BestTimeInSeconds = part.FinishTimeInSeconds;
                    _context.TrackRaces.Update(trackRace);
                    // UPdated???
                }
                
                var racer = await _context.Racers.FirstOrDefaultAsync(r => r.RacerId == part.RacerId);
                if (racer == null)
                {
                    throw new NotFoundException("Racer not found");
                }

                var participationAlready = await _context.RaceParticipations.FirstOrDefaultAsync(raceParticipation => raceParticipation.RacerId == racer.RacerId 
                    && raceParticipation.TrackRaceId == trackRace.TrackRaceId);
                if (participationAlready != null)
                {
                    throw new ConflictException("Participation already exists");
                }

                var participation = new RaceParticipation()
                {
                    TrackRaceId = trackRace.TrackRaceId,
                    RacerId = racer.RacerId,
                    FinishTimeInSeconds = part.FinishTimeInSeconds,
                    Position = part.Position,
                };
                _context.RaceParticipations.Add(participation);
            }
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
    }
}