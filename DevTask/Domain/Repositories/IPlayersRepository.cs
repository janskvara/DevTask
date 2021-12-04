using DevTask.Domain.Models;
using System.Threading.Tasks;

namespace DevTask.Domain.Repositories
{
    public interface IPlayersRepository
    {
        Task<Player> GetAsync(string userName);
        Task AddAsync(Player player);
    }
}
