namespace LotusAscend.Models
{
    // Models/Member.cs
    public class Member
    {
        public int Id { get; set; }
        public required string MobileNumber { get; set; }
        public bool IsVerified { get; set; } = false;
        public int Points { get; set; } = 0;

        // Navigation properties
        public Otp? Otp { get; set; }
        public ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();
    }
}
