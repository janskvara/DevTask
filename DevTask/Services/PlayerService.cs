using DevTask.Domain.Models;
using DevTask.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly IPlayersRepository playersRepository;

        public PlayerService(IPlayersRepository playersRepository)
        {
            this.playersRepository = playersRepository;
        }

        public async Task<Player> CreateNewPlayerAsync(string userName)
        {
            var player =  await playersRepository.GetAsync(userName);
            if(player != null)
            {
                return null;
            }

            player = new Player {
                Id = Guid.NewGuid(),
                UserName = userName,
                Wallet = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    Balance = 0,
                    Transactions = new List<Transaction>()
                },
            };

            await playersRepository.AddAsync(player);
            return player;
        }

        public async Task<Player> GetPlayerAsync(string userName)
        {
            var player = await playersRepository.GetAsync(userName);
            return player;
        }
    }
}
