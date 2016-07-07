﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KIB_Service.Models.dto;
using KIB_Service.Repositories.Interfaces;
using KIB_Service.TournamentMatchupEngine.Interface;
using KIB_Service.Models;
using KIB_Service.TournamentMatchupEngine;

namespace KIB_Service.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private ITournamentRepository tournamentRepository;
        private IPlayerRepository playerRepository;
        private IRoundRepository roundRepository;
        private IMatchupManager matchupManger;

        public TournamentController(ITournamentRepository tournamentRepository,
                                    IPlayerRepository playerRepository,
                                    IRoundRepository roundRepository,
                                    IMatchupManager matchupManger)
        {
            this.tournamentRepository = tournamentRepository;
            this.playerRepository = playerRepository;
            this.roundRepository = roundRepository;
            this.matchupManger = matchupManger;
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
            var matchups = roundRepository.GetAllMatchups(tournamentId);
            if(matchups == null || !matchups.Any())
            {
                return NoContent();
            }

            return Ok(matchups.Select(x => new RoundMatchupDto
            {
                RoundNumber = x.Key,
                RoundId = x.Any() ? x.First().RoundId : -1,
                Matchups = x.Select(y => new MatchupDto
                {
                    TableNumber = y.TableNumber,
                    Player1Id = y.Player1Id,
                    Player2Id = y.Player2Id
                }).ToList()
            }));
        }

        [HttpPost("{tournamentId}/matchups")]
        public IActionResult GenerateMatchupsForNextRound(int tournamentId)
        {
            Round round;
            try
            {
                round = matchupManger.GenerateMatchups(tournamentId);
            }
            catch (CantGenerateNewRoundException)
            {
                return BadRequest();
            }

            return Ok(round);
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

        [HttpGet("{tournamentId}/Player")]
        public IActionResult GetPlayers(int tournamentId)
        {
            var players = playerRepository.GetAllInTournament(tournamentId);
            return Ok(players.Select(p => p.ToPlayerDto()));
        }
    }
}
