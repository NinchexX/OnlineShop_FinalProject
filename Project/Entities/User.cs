using Microsoft.AspNetCore.Identity;

namespace Project.Entities
{
    public class User : IdentityUser
    {
        public User(string email,
                    string userName,
                    string firstName,
                    string lastName)
        {
            Email = email;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }

        public User()
        {
            
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Basket> Baskets { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
