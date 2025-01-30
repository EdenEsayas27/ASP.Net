using System.Threading.Tasks;
using Tvee.Models;

namespace Tvee.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(string userId);
    }
}