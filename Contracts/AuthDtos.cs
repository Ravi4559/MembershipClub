using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts
{
    public class AuthDtos
    {
        // Contracts/AuthDtos.cs


public record RegisterRequest([Required] string MobileNumber);
    public record VerifyRequest([Required] string MobileNumber, [Required] string Otp);
    public record AuthResponse(int MemberId, string MobileNumber, string Token);
}
}
