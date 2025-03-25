using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DB;
using Project.DTO.Product;
using Project.Entities;
using Project.Interfaces;

namespace Project.Services
{
    public class ProductService : IProductService
    {
        private readonly ProjectDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductService(ProjectDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }

        public async Task<List<GetProductResponseDTO>> GetProducts()
        {
            return await _dbContext.Products
                .Select(x => new GetProductResponseDTO(x.Id,
                                                        x.Name,
                                                        x.Description,
                                                        x.Price))
                .ToListAsync();
        }

        public async Task<GetProductResponseDTO?> GetProductById(int id)
        {
            return await _dbContext.Products
                .Where(x => x.Id == id)
                .Select(x => new GetProductResponseDTO(x.Id,
                                                        x.Name,
                                                        x.Description,
                                                        x.Price))
                .FirstOrDefaultAsync();
        }

        public async Task CreateProduct(CreateProductDTO request)
        {
            var product = new Product(request.Name,
                                      request.Description,
                                      request.Price);

            await _dbContext.Products.AddAsync(product);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(UpdateProductDTO request)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (product == default)
                throw new Exception("Product Not Found!");

            product.Update(request.Name,
                           request.Description,
                           request.Price);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == default)
                throw new Exception("Product Not Found!");

            _dbContext.Products.Remove(product);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RateProduct(RateProductDTO request)
        {
            IdentityOptions _options = new IdentityOptions();
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;

            var rating = new Rating(request.Id, userId, request.Rating);

            await _dbContext.Ratings.AddAsync(rating);

            await _dbContext.SaveChangesAsync();
        }
    }
}
