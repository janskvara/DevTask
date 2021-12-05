using DevTask.Domain.Dtos;
using DevTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // GET /player/GetBalance/{userNameOfPlayer}
        [Route("[action]/{userName}")]
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
    }
}
