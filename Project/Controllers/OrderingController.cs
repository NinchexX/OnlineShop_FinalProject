using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTO.Ordering;
using Project.Interfaces;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderingController : ControllerBase
    {
        private readonly IOrderingService _orderingService;

        public OrderingController(IOrderingService orderingService)
        {
            _orderingService = orderingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var products = await _orderingService.GetOrders();
            return Ok(products);
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetByUser()
        {
            var products = await _orderingService.GetOrdersByUser();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _orderingService.GetOrderById(id);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO request)
        {
            return Ok(await _orderingService.CreateOrder(request));
        }
    }
}
