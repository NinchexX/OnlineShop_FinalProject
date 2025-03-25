using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DB;
using Project.DTO.Ordering;
using Project.Entities;
using Project.Interfaces;

namespace Project.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly ProjectDbContext _dbContext;
        private readonly IHttpContextAccessor _context;

        public OrderingService(ProjectDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _context = httpContextAccessor;
        }

        public async Task<int> CreateOrder(CreateOrderDTO orderRequest)
        {
            IdentityOptions _options = new IdentityOptions();

            var userId = _context.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            var basket = await _dbContext.Baskets
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (basket == default)
                throw new Exception("Basket is empty!");

            var order = new Order
            {
                UserId = userId,
                TotalPrice = basket.Sum(x => x.Count * x.Product.Price), 
                CreateTime = DateTime.Now,
                OrderProducts = basket.Select(x => new OrderProduct
                {
                    ProductId = x.Product.Id,
                    Count = x.Count,
                    TotalPrice = x.Count * x.Product.Price
                }).ToList()
            };

            _dbContext.Orders.Add(order);

            _dbContext.Baskets.RemoveRange(basket);

            await _dbContext.SaveChangesAsync();

            return order.Id;
        }

        public async Task<GetOrderDTO?> GetOrderById(int orderId)
        {
            var order = await _dbContext.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            var orderDto = new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                CreateTime = order.CreateTime,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductDTO
                {
                    Id = op.ProductId,
                    Name = op.Product.Name,
                    Count = op.Count
                }).ToList()
            };

            return orderDto;
        }

        public async Task<List<GetOrderDTO>> GetOrdersByUser()
        {
            IdentityOptions _options = new IdentityOptions();

            var userId = _context.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            var orders = await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            return orders.Select(order => new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                CreateTime = order.CreateTime,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductDTO
                {
                    Id = op.ProductId,
                    Name = op.Product.Name,
                    Count = op.Count
                }).ToList()
            }).ToList();
        }

        public async Task<List<GetOrderDTO>> GetOrders()
        {
            IdentityOptions _options = new IdentityOptions();

            var userId = _context.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            var orders = await _dbContext.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            return orders.Select(order => new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                CreateTime = order.CreateTime,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductDTO
                {
                    Id = op.ProductId,
                    Name = op.Product.Name,
                    Count = op.Count
                }).ToList()
            }).ToList();
        }
    }
}