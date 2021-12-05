using DevTask.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Domain.Repositories
{
    public class PlayersRepository: IPlayersRepository
    {
        private readonly IList<Player> players = new List<Player>();

        public async Task<Player> GetAsync(string userName)
        {
            var player = players.Where(player => player.UserName == userName).SingleOrDefault();
            return await Task.FromResult(player);
        }

        public async Task AddAsync(Player player)
        {
            players.Add(player);
            await Task.CompletedTask;
        }
    }
}
