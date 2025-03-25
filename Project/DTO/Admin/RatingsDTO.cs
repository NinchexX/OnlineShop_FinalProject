namespace Project.DTO.Admin
{
    public class RatingsDTO
    {
        public RatingsDTO(int id, string userEmail, int rating)
        {
            Id = id;
            UserEmail = userEmail;
            Rating = rating;
        }

        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int Rating { get; set; }
    }
}
