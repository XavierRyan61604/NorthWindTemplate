namespace NorthWindTemplate.Models.ViewModels
{
    public class OrderWithDetailsViewModel
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustomerName { get; set; }
        public decimal? Freight { get; set; }
        public decimal TotalOrderValue { get; set; }
    }
}
