using DevTask.Domain.Models;

namespace DevTask.Domain.Dtos
{
    public static class Extensions
    {
        public static PlayerDto AsDto(this Player player)
        {
            return new PlayerDto()
            {
                Id = player.Id,
                UserName = player.UserName,
                WalletId = player.Wallet,
            };
        }
    }
}
