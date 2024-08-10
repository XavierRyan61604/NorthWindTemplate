using NorthWindTemplate.Data.Models;
using NorthWindTemplate.Models.ViewModels;

namespace NorthWindTemplate.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderWithDetailsViewModel> GetOrdersWithDetails();
    }
}
