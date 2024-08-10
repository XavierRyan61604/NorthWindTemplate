namespace NorthWindTemplate.Models.DTOs
{
    public class OrderFieldsByIdResponseDTO
    {
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
    }
}
