using System.Collections.Generic;

namespace Project.DTO.Admin
{
    public class GetRatingsDTO
    {
        public GetRatingsDTO(int id, string name)
        {
            Id = id;
            Name = name;
            Ratings = new List<RatingsDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<RatingsDTO> Ratings { get; set; }

        public void AddRating(RatingsDTO rating)
        {
            Ratings.Add(rating);
        }
    }
}
