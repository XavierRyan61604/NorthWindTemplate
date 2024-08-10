using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWindTemplate.Data.Models;
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
        public ActionResult<IEnumerable<OrderWithDetailsViewModel>> GetOrders()
        {
            var orders = _orderService.GetOrdersWithDetails();
            return Ok(orders);
        }
    }
}
