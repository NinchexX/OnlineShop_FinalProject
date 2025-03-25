using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Entities
{
    public class Basket
    {
        public Basket(string userId,
                      int productId,
                      int count)
        {
            UserId = userId;
            ProductId = productId;
            Count = count;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public void Update(int count)
        {
            Count = count;
        }
    }
}
