using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Entities
{
    public class Rating
    {
        public Rating(int productId, string userId, int ratingValue)
        {
            ProductId = productId;
            UserId = userId;
            RatingValue = ratingValue;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int RatingValue { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
