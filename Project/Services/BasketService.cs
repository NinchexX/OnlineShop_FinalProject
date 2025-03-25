using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DB;
using Project.DTO.Basket;
using Project.DTO.Product;
using Project.Entities;
using Project.Interfaces;

namespace Project.Services
{
    public class BasketService : IBasketService
    {
        private readonly ProjectDbContext _dbContext;
        private IHttpContextAccessor _context;

        public BasketService(ProjectDbContext dbContext, IHttpContextAccessor context)
        {
            _dbContext = dbContext;
            _context = context;
        }

        public async Task<List<GetBasketResponseDTO>> GetBasket()
        {
            IdentityOptions _options = new IdentityOptions();

            var userId = _context.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            return await _dbContext.Baskets
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .Select(x => new GetBasketResponseDTO(x.Id,
                                                        x.ProductId,
                                                        x.Count,
                                                        x.Product.Price * x.Count))
                .ToListAsync();
        }

        public async Task AddToBasket(AddToBasketRequestDTO request)
        {
            IdentityOptions _options = new IdentityOptions();

            var userId = _context.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (product == default)
                throw new Exception("Product Not Found!");

            var basket = new Basket(userId,
                                      product.Id,
                                      request.Count);

            await _dbContext.Baskets.AddAsync(basket);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBasket(UpdateBasketRequestDTO request)
        {
            IdentityOptions _options = new IdentityOptions();

            var userId = _context.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            var basket = await _dbContext.Baskets.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == userId);

            if (basket == default)
                throw new Exception("Product In Basket Not Found!");

            basket.Update(request.Count);

            _dbContext.Baskets.Update(basket);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBasket(int id)
        {
            var basket = await _dbContext.Baskets.FirstOrDefaultAsync(x => x.Id == id);

            if (basket == default)
                throw new Exception("Basket Not Found!");

            _dbContext.Baskets.Remove(basket);

            await _dbContext.SaveChangesAsync();
        }
    }
}
