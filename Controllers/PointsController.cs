using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LotusAscend.Contracts.PointsDtos;

[ApiController]
[Route("api/[controller]")]

[Authorize] // This protects all endpoints in this controller 
public class PointsController : ControllerBase
{
    private readonly IPointsService _pointsService;

    public PointsController(IPointsService pointsService)
    {
        _pointsService = pointsService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddPoints(AddPointsRequest request)
    {
        // Get the member ID from the JWT token's 'sub' claim
        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var success = await _pointsService.AddPointsAsync(memberId, request);
        if (!success)
        {
            return BadRequest("Could not add points.");
        }
        return Ok(new { Message = "Points added successfully." });
    }

    [HttpGet]
    public async Task<IActionResult> GetPoints()
    {
        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var memberDetails = await _pointsService.GetMemberPointsAsync(memberId);

        if (memberDetails == null)
        {
            return NotFound("Member not found.");
        }
        return Ok(memberDetails);
    }
}
