using DevTask.Domain.Models;
using System.Threading.Tasks;

namespace DevTask.Services
{
    public interface IPlayerService
    {
        Task<Player> CreateNewPlayerAsync(string userName);

        Task<Player> GetPlayerAsync(string userName);
    }
}
