// Services/MemberService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using LotusAscend.Interfaces;
using LotusAscend.Models;
using static LotusAscend.Contracts.AuthDtos;

public class MemberService : IMemberService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public MemberService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<int> RegisterAsync(RegisterRequest request)
    {
        var existingMember = await _context.Members
            .FirstOrDefaultAsync(m => m.MobileNumber == request.MobileNumber);

        if (existingMember != null)
        {
            // For simplicity, we can re-issue an OTP. 
            // A real-world app might have stricter rules.
            return existingMember.Id;
        }

        var newMember = new Member { MobileNumber = request.MobileNumber };

        // Create a dummy OTP [cite: 17, 18]
        var otp = new Otp
        {
            Code = "1234",
            Expiry = DateTime.UtcNow.AddMinutes(5),
            Member = newMember
        };

        _context.Members.Add(newMember);
        _context.Otps.Add(otp);
        await _context.SaveChangesAsync();

        return newMember.Id;
    }
    // (Continuing inside the MemberService class)
    public async Task<AuthResponse?> VerifyOtpAsync(VerifyRequest request)
    {
        var member = await _context.Members.Include(m => m.Otp)
            .FirstOrDefaultAsync(m => m.MobileNumber == request.MobileNumber);

        if (member == null || member.Otp == null || member.Otp.Code != request.Otp || member.Otp.Expiry < DateTime.UtcNow)
        {
            return null; // OTP is invalid, expired, or member doesn't exist
        }

        member.IsVerified = true;
        _context.Otps.Remove(member.Otp); // OTP is single-use
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(member);

        return new AuthResponse(member.Id, member.MobileNumber, token);
    }

    private string GenerateJwtToken(Member member)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}