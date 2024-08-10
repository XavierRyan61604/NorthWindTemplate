using NorthWindTemplate.Models.DTOs;

namespace NorthWindTemplate.Services
{
    public interface IOrderService
    {
        //取得訂單資訊及訂單總金額
        IEnumerable<OrderWithDetailsResponseDTO> GetOrdersWithDetails(GetOrdersPaginationRequestDTO req);
        //取得訂單總筆數
        int GetOrdersCount();
        //取得特定訂單資訊
        OrderFieldsByIdResponseDTO GetOrderFieldsById(int orderId);
        //更新特定訂單資訊
        void UpdateOrderFields(OrderFieldsUpdateDTO updateDTO);
        //刪除特定訂單
        bool DeleteOrder(int orderId);
        //新增訂單
        bool AddOrder(AddOrderRequestDTO addOrderRequestDTO);
    }
}
