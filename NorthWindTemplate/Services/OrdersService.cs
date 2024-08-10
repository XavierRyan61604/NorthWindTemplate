using Microsoft.EntityFrameworkCore;
using NorthWindTemplate.Data;
using NorthWindTemplate.Data.Models;
using NorthWindTemplate.Models.ViewModels;

namespace NorthWindTemplate.Services
{
    public class OrdersService : IOrderService
    {
        private readonly NorthwindContext _context;

        public OrdersService(NorthwindContext context)
        {
            _context = context;
        }
        //取得訂單資訊及訂單總金額
        public IEnumerable<OrderWithDetailsViewModel> GetOrdersWithDetails()
        {
            var ordersWithDetails = _context.Orders
                .Join(_context.OrderDetails,
                      order => order.OrderId,
                      orderDetail => orderDetail.OrderId,
                      (order, orderDetail) => new { order, orderDetail })
                .GroupBy(o => new
                {
                    o.order.OrderId,
                    o.order.OrderDate,
                    o.order.Customer.CompanyName,
                    o.order.Freight
                })
                .Select(g => new OrderWithDetailsViewModel
                {
                    OrderId = g.Key.OrderId,
                    OrderDate = g.Key.OrderDate,
                    CustomerName = g.Key.CompanyName,
                    Freight = g.Key.Freight,
                    TotalOrderValue = g.Sum(od => od.orderDetail.Quantity * od.orderDetail.UnitPrice)
                })
                .ToList();
            return ordersWithDetails;
        }
    }
}
