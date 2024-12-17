using System.Threading.Tasks;

using ppawproject.Database.Entities;

namespace ppawproject.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(User user);
        Task<User> UpdateUser(int id, User updatedUser);
        Task<bool> DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
