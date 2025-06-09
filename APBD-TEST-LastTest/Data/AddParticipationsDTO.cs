namespace APBD_TEST_LastTest.Data;

public class AddParticipationsDTO
{
    public string RaceName { get; set; }
    public string TrackName { get; set; }
    public List<AParticipationDTO> Participations { get; set; }
}

public class AParticipationDTO
{
    public int RacerId { get; set; }
    public int Position { get; set; }
    public int FinishTimeInSeconds { get; set; }
}