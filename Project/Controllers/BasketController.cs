using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTO.Basket;
using Project.DTO.Product;
using Project.Interfaces;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var products = await _basketService.GetBasket();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody] AddToBasketRequestDTO request)
        {
            await _basketService.AddToBasket(request);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketRequestDTO request)
        {
            await _basketService.UpdateBasket(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            await _basketService.DeleteBasket(id);
            return Ok();
        }
    }
}
