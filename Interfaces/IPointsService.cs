// Interfaces/IPointsService.cs
using LotusAscend.Contracts;
using static LotusAscend.Contracts.PointsDtos;

public interface IPointsService
{
    Task<bool> AddPointsAsync(int memberId, AddPointsRequest request);
    Task<MemberResponse?> GetMemberPointsAsync(int memberId);
}