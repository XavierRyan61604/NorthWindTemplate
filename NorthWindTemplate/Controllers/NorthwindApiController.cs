using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWindTemplate.Data.Models;
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

        // GET: api/NorthwindApi
        [HttpGet("orders")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
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
    }
}
