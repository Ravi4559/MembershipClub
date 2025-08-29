using static LotusAscend.Contracts.AuthDtos;

public interface IMemberService
{
    Task<int> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> VerifyOtpAsync(VerifyRequest request);
}
