using APBD_TEST_LastTest.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_TEST_LastTest.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Race> Races { get; set; }
    public DbSet<RaceParticipation> RaceParticipations { get; set; }
    public DbSet<TrackRace> TrackRaces { get; set; }
    public DbSet<Racer> Racers { get; set; }
    public DbSet<Track> Tracks { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Race>().HasData(new List<Race>()
        {
            new Race(){RaceId = 1, Name = "Some Race", Location = "Some location", Date = DateTime.Parse("2025-05-01")},
            new Race(){RaceId = 2, Name = "Another Race", Location = "Another location", Date = DateTime.Parse("2025-05-02")},
            new Race(){RaceId = 3, Name = "Yet another Race", Location = "Yet another location", Date = DateTime.Parse("2025-05-03")},
        });

        modelBuilder.Entity<Track>().HasData(new List<Track>()
        {
            new Track(){TrackId = 1, Name = "1 Track", LengthInKM = 10.02},
            new Track(){TrackId = 2, Name = "2 Track", LengthInKM = 20.09},
            new Track(){TrackId = 3, Name = "3 Track", LengthInKM = 30.12},
            new Track(){TrackId = 4, Name = "4 Track", LengthInKM = 40.12},
        });

        modelBuilder.Entity<Racer>().HasData(new List<Racer>()
        {
            new Racer(){RacerId = 1, FirstName = "Gleb", LastName = "Denisov"},
            new Racer(){RacerId = 2, FirstName = "John", LastName = "Doe"},
            new Racer(){RacerId = 3, FirstName = "Janette", LastName = "Doj"},
        });

        modelBuilder.Entity<TrackRace>().HasData(new List<TrackRace>()
        {
            new TrackRace(){TrackRaceId = 1, TrackId = 1, RaceId = 1, Laps = 10, BestTimeInSeconds = null},
            new TrackRace(){TrackRaceId = 2, TrackId = 2,RaceId = 2, Laps = 20, BestTimeInSeconds = 29},
            new TrackRace(){TrackRaceId = 3, TrackId = 3,RaceId = 3, Laps = 10, BestTimeInSeconds = null},
            new TrackRace(){TrackRaceId = 4, TrackId = 2,RaceId = 2, Laps = 10, BestTimeInSeconds = 10},
        });

        modelBuilder.Entity<RaceParticipation>().HasData(new List<RaceParticipation>()
        {
            new RaceParticipation(){TrackRaceId = 1, RacerId = 1, FinishTimeInSeconds = 10, Position = 1},
            new RaceParticipation(){TrackRaceId = 2, RacerId = 2, FinishTimeInSeconds = 19, Position = 1},
            new RaceParticipation(){TrackRaceId = 3, RacerId = 3, FinishTimeInSeconds = 29, Position = 1},
            new RaceParticipation(){TrackRaceId = 4, RacerId = 1, FinishTimeInSeconds = 30, Position = 1},
        });
    }
}