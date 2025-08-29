using System.ComponentModel.DataAnnotations;

namespace LotusAscend.Contracts
{
    public class PointsDtos
    {

     public record AddPointsRequest([Required][Range(1, double.MaxValue)] decimal PurchaseAmount);
    
    }
}
