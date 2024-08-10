using Microsoft.EntityFrameworkCore;
using NorthWindTemplate.Data;
using NorthWindTemplate.Data.Models;
using NorthWindTemplate.Models.DTOs;
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
        public IEnumerable<OrderWithDetailsResponseDTO> GetOrdersWithDetails(GetOrdersPaginationRequestDTO r)
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
                    o.order.Freight,
                    o.order.ShipName,
                    o.order.ShipAddress,
                })
                .Select(g => new OrderWithDetailsResponseDTO
                {
                    OrderId = g.Key.OrderId,
                    OrderDate = g.Key.OrderDate,
                    CustomerName = g.Key.CompanyName,
                    Freight = g.Key.Freight,
                    ShipAddress = g.Key.ShipAddress,
                    ShipName = g.Key.ShipName,
                    TotalOrderValue = g.Sum(od => od.orderDetail.Quantity * od.orderDetail.UnitPrice)
                })
                .OrderByDescending(o => o.OrderId)
                .Skip((r.PageNumber - 1) * r.PageSize)
                .Take(r.PageSize)
                .ToList();
            return ordersWithDetails;
        }
        //取得訂單總筆數
        public int GetOrdersCount()
        {
            return _context.Orders.Count();
        }
        //取得特定訂單資訊
        public OrderFieldsByIdResponseDTO GetOrderFieldsById(int orderId)
        {
            var order = _context.Orders.Find(orderId);

            if (order == null)
            {
                return null;
            }

            return new OrderFieldsByIdResponseDTO
            {
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress
            };
        }
        //更新特定訂單資訊
        public void UpdateOrderFields(OrderFieldsUpdateDTO updateDTO)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == updateDTO.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {updateDTO.OrderId} not found.");
            }

            // 更新订单字段
            if (updateDTO.Freight.HasValue)
            {
                order.Freight = updateDTO.Freight.Value;
            }

            if (!string.IsNullOrWhiteSpace(updateDTO.ShipName))
            {
                order.ShipName = updateDTO.ShipName;
            }

            if (!string.IsNullOrWhiteSpace(updateDTO.ShipAddress))
            {
                order.ShipAddress = updateDTO.ShipAddress;
            }

            _context.SaveChanges();
        }
        //刪除特定訂單
        public bool DeleteOrder(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return false;
            }

            _context.OrderDetails.RemoveRange(order.OrderDetails);

            _context.Orders.Remove(order);

            //_context.SaveChanges();

            return true;
        }
        //新增訂單
        public bool AddOrder(AddOrderRequestDTO addOrderRequestDTO)
        {
            var randomCustomerId = _context.Customers
                .OrderBy(c => Guid.NewGuid())
                .Select(c => c.CustomerId)
                .FirstOrDefault();

            var randomEmployeeId = _context.Employees
                .OrderBy(e => Guid.NewGuid())
                .Select(e => e.EmployeeId)
                .FirstOrDefault();
            var randomProductId = _context.Products
                .OrderBy(e => Guid.NewGuid())
                .Select(e => e.ProductId)
                .FirstOrDefault();

            var order = new Order
            {
                CustomerId = randomCustomerId,
                EmployeeId = randomEmployeeId,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7),
                ShippedDate = DateTime.Now.AddDays(14),
                ShipVia = 1,
                Freight = 32.38m,
                ShipCity = "Reims",
                ShipRegion = "NULL",
                ShipPostalCode = "51100",
                ShipCountry = "France",
                ShipName = addOrderRequestDTO.ShipName,
                ShipAddress = addOrderRequestDTO.ShipAddress,
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    int generatedOrderId = order.OrderId;

                    var orderDetail = new OrderDetail
                    {
                        OrderId = generatedOrderId, 
                        ProductId = randomProductId, 
                        UnitPrice = 20.00M,
                        Quantity = 5,
                        Discount = 0.0f
                    };

                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }
    }
}
