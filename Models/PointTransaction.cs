namespace LotusAscend.Models
{
    
    public class PointTransaction
    {
        public int Id { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int PointsAdded { get; set; }
        public DateTime TransactionDate { get; set; }
        public int MemberId { get; set; }

        
        public Member? Member { get; set; }
    }
}
