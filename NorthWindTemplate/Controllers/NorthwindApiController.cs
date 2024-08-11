using Microsoft.AspNetCore.Mvc;
using NorthWindTemplate.Models.DTOs;
using NorthWindTemplate.Models.ViewModels;
using NorthWindTemplate.Services;

namespace NorthWindTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NorthwindApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public NorthwindApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public ActionResult<IEnumerable<PaginatedResponseViewModel<OrderWithDetailsResponseDTO>>> GetOrders([FromQuery] GetOrdersPaginationRequestDTO req)
        {
            var totalRecords = _orderService.GetOrdersCount();

            var orders = _orderService.GetOrdersWithDetails(req);

            var response = new PaginatedResponseViewModel<OrderWithDetailsResponseDTO>
            {
                Draw = req.draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = orders
            };
            return Ok(response);
        }

        [HttpGet("orderByID")]
        [ResponseCache(Duration = 3, Location = ResponseCacheLocation.Client)]
        public ActionResult<OrderFieldsByIdResponseDTO> GetOrderFieldsById([FromQuery] int orderID)
        {
            var response = _orderService.GetOrderFieldsById(orderID);

            return Ok(response);
        }

        [HttpPut("updateOrderFields")]
        public ActionResult UpdateOrderFields([FromBody] OrderFieldsUpdateDTO updateDTO)
        {
            _orderService.UpdateOrderFields(updateDTO);
            return NoContent();
        }

        [HttpDelete("orders/{orderId}")]
        public ActionResult DeleteOrder(int orderId)
        {
            // 调用服务层删除方法
            var success = _orderService.DeleteOrder(orderId);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound(); 
            }
        }

        [HttpPost("createOrder")]
        public IActionResult CreateOrder([FromBody] AddOrderRequestDTO orderRequest)
        {
            if (_orderService.AddOrder(orderRequest))
            {
                return Ok();
            }

            return BadRequest("Failed to create order.");
        }
    }
}
