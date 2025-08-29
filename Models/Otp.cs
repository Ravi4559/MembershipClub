namespace LotusAscend.Models
{
    // Models/Otp.cs
    public class Otp
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public DateTime Expiry { get; set; }
        public int MemberId { get; set; }

        // Navigation property
        public Member? Member { get; set; }
    }
}
