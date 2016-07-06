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

        public TournamentController(ITournamentRepository tournamentRepository,
                                    IPlayerRepository playerRepository)
        {
            this.tournamentRepository = tournamentRepository;
            this.playerRepository = playerRepository;
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
            throw new NotImplementedException();
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
