using Project.DTO.Product;

namespace Project.Interfaces
{
    public interface IProductService
    {
        Task<List<GetProductResponseDTO>> GetProducts();
        Task<GetProductResponseDTO?> GetProductById(int id);
        Task CreateProduct(CreateProductDTO request);
        Task UpdateProduct(UpdateProductDTO request);
        Task DeleteProduct(int id);
        Task RateProduct(RateProductDTO request);
    }
}
