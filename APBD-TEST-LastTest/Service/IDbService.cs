using APBD_TEST_LastTest.DTOs;

namespace APBD_TEST_LastTest.Service;

public interface IDbService
{
    Task<GetRacesDTO> GetParticipations(int racerId);
}