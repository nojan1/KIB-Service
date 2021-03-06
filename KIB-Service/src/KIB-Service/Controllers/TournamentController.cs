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


            var currentRound = roundRepository.GetCurrentRound(tournamentId);
            var scores = roundRepository.GetScoresForTournament(tournamentId);

            return Ok(matchups.Select(x => new RoundMatchupDto
            {
                RoundNumber = x.Key,
                RoundId = x.Any() ? x.First().RoundId : -1,
                Public = currentRound == null ? false : x.Any() && currentRound.Id == x.First().RoundId ? currentRound.Public : true,
                Matchups = x.Select(y => new MatchupDto
                {
                    Id = y.Id,
                    TableNumber = y.TableNumber,
                    Player1Id = y.Player1Id,
                    Player2Id = y.Player2Id ?? -1,
                    Player1Score = scores.SingleOrDefault(s => s.MatchupId == y.Id && s.PlayerId == y.Player1Id)?.Amount ?? 0,
                    Player2Score = scores.SingleOrDefault(s => s.MatchupId == y.Id && s.PlayerId == y.Player2Id)?.Amount ?? 0
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
        public IActionResult GetScoreboard(int tournamentId)
        {
            var scores = roundRepository.GetScoresForTournament(tournamentId);
            var players = playerRepository.GetAllInTournament(tournamentId);

            return Ok(players.Select(p => new PlayerScoreDto
            {
                PlayerId = p.Id,
                Name = p.Name,
                Affiliation = p.Affiliation,
                Score = scores.Where(s => s.PlayerId == p.Id).Sum(s => s.Amount),
                MatchScores = scores.Where(s => s.PlayerId == p.Id && s.MatchupId.HasValue).OrderBy(s => s.MatchupId).Select(s => new MatchScore
                {
                    Score = s.Amount
                }).ToList()
            }).OrderByDescending(x => x.Score));
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

        [HttpDelete("{tournamentId}/Player/{playerId}")]
        public IActionResult DeletePlayer(int tournamentId, int playerId)
        {
            var tournament = tournamentRepository.Get(tournamentId);
            if(tournament == null)
            {
                return BadRequest();
            }

            var round = roundRepository.GetCurrentRound(tournamentId);
            if(round == null)
            {
                playerRepository.Delete(playerId);
            }else
            {
                playerRepository.Disable(playerId);
            }

            return Ok();
        }

        [HttpGet("{tournamentId}/Player")]
        public IActionResult GetPlayers(int tournamentId)
        {
            var players = playerRepository.GetAllInTournament(tournamentId);
            return Ok(players.Select(p => p.ToPlayerDto()));
        }

        [HttpPost("{tournamentId}/score/{matchupId}")]
        public IActionResult ReportScore(int tournamentId, int matchupId, [FromBody]MatchupDto score)
        {
            if((score.Player1Score == 0 && score.Player2Score == 0) || score.Player1Score < 0 ||score.Player2Score < 0)
            {
                return BadRequest();
            }

            roundRepository.SetScore(matchupId, score.Player1Score, score.Player2Score);

            return Ok();
        }

        [HttpPost("{tournamentId}/round/{roundId}/public")]
        public IActionResult MakeRoundPublic(int tournamentId, int roundId)
        {
            roundRepository.MakePublic(roundId);
            return Ok();
        }
    }
}
