using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KIB_Service.Models.dto;
using KIB_Service.Repositories.Interfaces;

namespace KIB_Service.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private ITournamentRepository tournamentRepository;
        private IPlayerRepository playerRepository;
        private IRoundRepository roundRepository;

        public TournamentController(ITournamentRepository tournamentRepository,
                                    IPlayerRepository playerRepository,
                                    IRoundRepository roundRepository)
        {
            this.tournamentRepository = tournamentRepository;
            this.playerRepository = playerRepository;
            this.roundRepository = roundRepository;
        }

        [HttpGet]
        public IEnumerable<TournamentDto> Get()
        {
            return tournamentRepository.List().Select(t => t.ToTournamentDto());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tournament = tournamentRepository.Get(id);
            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(tournament.ToTournamentDto());
        }

        [HttpPost("")]
        public IActionResult CreateTournament([FromBody]TournamentDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var tournament = tournamentRepository.Add(value);

            return Ok(tournament.ToTournamentDto());
        }

        [HttpGet("{tournamentId}/matchups")]
        public IActionResult GetMatchups(int tournamentId)
        {
            var round = roundRepository.GetCurrentRound(tournamentId);
            if(round == null)
            {
                return NoContent();
            }

            return Ok(round.Matchups);
        }

        [HttpGet("{tournamentId}/score")]
        public IActionResult GetHighscore(int tournamentId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{tournamentId}/Player")]
        public IActionResult AddPlayer(int tournamentId, [FromBody]PlayerDto player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdPlayer = playerRepository.Add(tournamentId, player);

            return Ok(createdPlayer.ToPlayerDto());
        }
    }
}
