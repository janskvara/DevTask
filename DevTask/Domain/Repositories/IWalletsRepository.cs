using DevTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Domain.Repositories
{
    public interface IWalletsRepository
    {
        Task<Wallet> GetAsync(Guid id);
        Task AddAsync(Wallet wallet);
    }
}
