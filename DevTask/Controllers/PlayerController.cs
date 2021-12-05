using DevTask.Domain.Dtos;
using DevTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevTask.Domain.Models;
using System.Collections.Generic;

namespace DevTask.Controllers
{
    [ApiController]
    [Route("player")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService playerService;
        public PlayerController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        // POST /player
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> Registration(RegistrationOfPlayerDto registrationDto)
        {
            var player = await playerService.CreateNewPlayerAsync(registrationDto.UserName);
            if (player == null)
            {
                return NoContent();
            }
            return CreatedAtAction(nameof(Registration), new { id = player.Id }, player.AsDto());
        }

        // POST /player/{userName}/Transaction
        [Route("{userName}/[action]")]
        [HttpPost]
        public async Task<ActionResult<string>> Transaction(string userName, RegistrationOfTransactionDto registrationDto)
        {
            var stateOfTransaction = await playerService.AddTransactionAsync(userName, registrationDto.ToModel());
            if (stateOfTransaction == EStateOfTransaction.UserDoesntExist || stateOfTransaction == EStateOfTransaction.WalletDoesntFound)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(Registration), new {}, stateOfTransaction.ToString());
        }

        // GET /player/{userName}/GetBalance/
        [Route("{userName}/[action]")]
        [HttpGet()]
        public async Task<ActionResult<decimal>> GetBalance(string userName)
        {
            var balance = await playerService.GetPlayerBalanceAsync(userName);
            if (balance == decimal.MinusOne)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetBalance), new {}, balance);
        }

        // GET /player/{userName}/GetTransactions
        [Route("{userName}/[action]")]
        [HttpGet()]
        public async Task<ActionResult<Transaction>> GetTransactions(string userName)
        {
            var transactions = await playerService.GetPlayerTransactionseAsync(userName);
            if (transactions == null)
            {
                return NotFound();
            }
            IList<TransactionDto> transactionDtos = new List<TransactionDto>();
            foreach (Transaction transaction in transactions)
            {
                transactionDtos.Add(transaction.AsDto());
            }
            return CreatedAtAction(nameof(GetTransactions), new { }, transactionDtos);
        }
    }
}
