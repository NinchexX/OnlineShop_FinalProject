namespace Project.DTO.Basket
{
    public class GetBasketResponseDTO
    {
        public GetBasketResponseDTO(int id,
                                    int productId,
                                    int count,
                                    decimal price)
        {
            Id = id;
            ProductId = productId;
            Count = count;
            Price = price;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
