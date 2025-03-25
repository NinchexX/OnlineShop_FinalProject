namespace Project.Entities
{
    public class Product
    {
        public Product(string name,
                       string description,
                       decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public void Update(string name,
                           string description,
                           decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
