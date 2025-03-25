using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
