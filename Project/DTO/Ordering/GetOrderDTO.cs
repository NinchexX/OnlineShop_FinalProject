namespace Project.DTO.Ordering
{
    public class GetOrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }
    }

    public class OrderProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
