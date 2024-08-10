using NorthWindTemplate.Models.DTOs;

namespace NorthWindTemplate.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderWithDetailsResponseDTO> GetOrdersWithDetails(GetOrdersPaginationRequestDTO req);
        int GetOrdersCount();
    }
}
