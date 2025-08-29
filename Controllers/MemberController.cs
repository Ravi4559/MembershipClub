// Controllers/MemberController.cs
using Microsoft.AspNetCore.Mvc;
//using LotusAscend.Contracts;
//using LotusAscend.Interfaces;
using static LotusAscend.Contracts.AuthDtos;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var memberId = await _memberService.RegisterAsync(request);
        return Ok(new { Message = "OTP sent successfully. Please verify.", MemberId = memberId });
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify(VerifyRequest request)
    {
        var response = await _memberService.VerifyOtpAsync(request);
        if (response == null)
        {
            return Unauthorized("Invalid OTP or mobile number.");
        }
        return Ok(response);
    }
}