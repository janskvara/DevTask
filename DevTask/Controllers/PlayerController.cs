using DevTask.Domain.Dtos;
using DevTask.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<PlayerDto>> RegistrationAsync(RegistrationDto registrationDto)
        {
            var player = await playerService.CreateNewPlayerAsync(registrationDto.UserName);
            if (player == null)
            {
                return NoContent();
            }
            return CreatedAtAction(nameof(RegistrationAsync), new { id = player.Id }, player.AsDto());
        }

        // GET /player/GetBalance/{userNameOfPlayer}
        [Route("[action]/{userName}")]
        [HttpGet()]
        public async Task<ActionResult<PlayerDto>> GetBalanceAsync(string userName)
        {
            var player = await playerService.GetPlayerAsync(userName);
            if (player == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetBalanceAsync), new { id = player.Id }, player.AsDto());
        }
    }
}
