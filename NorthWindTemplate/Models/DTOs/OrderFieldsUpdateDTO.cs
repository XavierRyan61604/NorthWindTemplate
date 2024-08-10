namespace NorthWindTemplate.Models.DTOs
{
    public class OrderFieldsUpdateDTO
    {
        public int OrderId { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
    }
}
