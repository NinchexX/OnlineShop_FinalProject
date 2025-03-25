using Project.DTO.Ordering;

namespace Project.Interfaces
{
    public interface IOrderingService
    {
        Task<int> CreateOrder(CreateOrderDTO orderRequest);
        Task<GetOrderDTO?> GetOrderById(int orderId);
        Task<List<GetOrderDTO>> GetOrdersByUser();
        Task<List<GetOrderDTO>> GetOrders();
    }
}
