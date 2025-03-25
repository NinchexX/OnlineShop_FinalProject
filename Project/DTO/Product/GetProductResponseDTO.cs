namespace Project.DTO.Product
{
    public class GetProductResponseDTO
    {
        public GetProductResponseDTO(int id,
                                     string name,
                                     string description,
                                     decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
