using Project.DTO.Admin;
using Project.Entities;

namespace Project.Interfaces
{
    public interface IAdminService
    {
        Task CreateUser(string username, string email, string password, string role);
        Task AssignRoleToUser(string id, string role);
        Task EditUser(string id, string firstName, string lastName);
        Task<GetRatingsDTO> GetRatings(int id);
    }
}
