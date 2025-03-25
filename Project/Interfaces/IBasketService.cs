using Project.DTO.Basket;

namespace Project.Interfaces
{
    public interface IBasketService
    {
        Task<List<GetBasketResponseDTO>> GetBasket();
        Task AddToBasket(AddToBasketRequestDTO request);
        Task UpdateBasket(UpdateBasketRequestDTO request);
        Task DeleteBasket(int id);
    }
}
