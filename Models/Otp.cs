namespace LotusAscend.Models
{
    
    public class Otp
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public DateTime Expiry { get; set; }
        public int MemberId { get; set; }

        
        public Member? Member { get; set; }
    }
}
