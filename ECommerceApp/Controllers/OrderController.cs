using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await _orderService.CreateOrderAsync(order);
            return Ok();
        }
    }
}
