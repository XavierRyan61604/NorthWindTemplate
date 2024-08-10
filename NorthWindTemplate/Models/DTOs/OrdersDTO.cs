namespace NorthWindTemplate.Models.DTOs
{
    public class GetOrdersPaginationRequestDTO
    {
        public int draw { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class OrderWithDetailsResponseDTO
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? Freight { get; set; }
        public decimal TotalOrderValue { get; set; }
    }
}
