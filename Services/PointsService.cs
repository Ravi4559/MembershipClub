using Microsoft.EntityFrameworkCore;
using LotusAscend.Contracts;
using LotusAscend.Models;
using static LotusAscend.Contracts.PointsDtos;

public class PointsService : IPointsService
{
    private readonly AppDbContext _context;

    public PointsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPointsAsync(int memberId, AddPointsRequest request)
    {
        var member = await _context.Members.FindAsync(memberId);
        if (member == null) return false;

        //[cite_start]// Rule: 10 points for every ₹100 purchase [cite: 27]
        int pointsToAdd = (int)(request.PurchaseAmount / 100) * 10;

        if (pointsToAdd > 0)
        {
            member.Points += pointsToAdd;

            var transaction = new PointTransaction
            {
                MemberId = memberId,
                PurchaseAmount = request.PurchaseAmount,
                PointsAdded = pointsToAdd,
                TransactionDate = DateTime.UtcNow
            };
            _context.PointTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
        return true;
    }

    public async Task<MemberResponse?> GetMemberPointsAsync(int memberId)
    {
        var member = await _context.Members.FindAsync(memberId);

        if (member == null) return null;

        // Using a DTO to return data
        return new MemberResponse(member.Id, member.MobileNumber, member.Points);
    }
}
