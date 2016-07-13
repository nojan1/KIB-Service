using KIB_Service.Models;
using KIB_Service.Repositories.Interfaces;
using KIB_Service.TournamentMatchupEngine.Interface;
using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine
{
    public class MatchupManager : IMatchupManager
    {
        private IRoundRepository roundRepository;
        private IPlayerRepository playerRepository;

        public MatchupManager(IRoundRepository roundRepository,
                              IPlayerRepository playerRepository)
        {
            this.roundRepository = roundRepository;
            this.playerRepository = playerRepository;
        }

        public bool CanGenerateNextRound(int tournamentId)
        {
            var currentRound = roundRepository.GetCurrentRound(tournamentId);
            if (currentRound == null)
                return true;

            var scores = roundRepository.GetScoresForTournament(tournamentId);
            var scoresForCurrentRound = scores.Where(s => currentRound.Matchups.Select(m => m.Id).Contains(s.MatchupId)).ToList();

            return scoresForCurrentRound.Count == currentRound.Matchups.Sum(m => m.Player2Id.HasValue ? 2 : 1);
        }

        public Round GenerateMatchups(int tournamentId)
        {
            if (!CanGenerateNextRound(tournamentId))
            {
                throw new CantGenerateNewRoundException();
            }

            var previousMatchups = roundRepository.GetAllMatchups(tournamentId);
            var players = playerRepository.GetAllInTournament(tournamentId);
            var scores = roundRepository.GetScoresForTournament(tournamentId);

            var contestants = players.Where(p => p.Active).Select(p => new Contestant
            {
                Identifier = p.Id,
                Affiliation = p.Affiliation,
                PreviousOpponents = ExtractPreviousOpponentsForPlayer(p, previousMatchups, players, scores),
                Score = scores.Where(s => s.PlayerId == p.Id).Sum(s => s.Amount),
                CompensationPoints = p.CompensationPoints
            }).OrderBy(c => c.Score).ToList();

            if (contestants.Count <= 1)
            {
                throw new Exception("Not enough active player in tournament to generate round matchups");
            }

            var engine = new MatchupEngine();
            var contestantMatchups = engine.GenerateMatchup(contestants.Count % 2 == 0 ? contestants
                                                                                       : contestants.Skip(1).ToList());

            var round = roundRepository.Add(new Round
            {
                RoundNumber = previousMatchups.Any() ? previousMatchups.Max(x => x.Key) + 1 : 1,
                TournamentId = tournamentId
            });

            roundRepository.AddMatchupsToRound(round.Id, contestantMatchups.Select(x => new Matchup
            {
                RoundId = round.Id,
                TableNumber = x.Table,
                Player1Id = x.Contestant1.Identifier,
                Player2Id = x.Contestant2.Identifier
            }).ToList());

            if (contestants.Count % 2 != 0)
            {
                //The left over player gets a pseudo matchup and score
                var leftOverContestant = contestants.First();
                roundRepository.AddPseudoMatchupToRound(round.Id, leftOverContestant.Identifier);
            }

            return roundRepository.GetCurrentRound(tournamentId);
        }

        private ICollection<ContestantWithoutPreviouseOpponents> ExtractPreviousOpponentsForPlayer(Player player, IEnumerable<IGrouping<int, Matchup>> allMatchups, ICollection<Player> allPlayers, ICollection<Score> allScores)
        {
            var previousOpponents = new List<Player>();

            foreach (var roundMatchups in allMatchups)
            {
                foreach (var matchup in roundMatchups)
                {
                    if (matchup.Player1Id == player.Id)
                    {
                        if (matchup.Player2Id.HasValue)
                        {
                            previousOpponents.Add(allPlayers.Single(p => p.Id == matchup.Player2Id));
                        }
                    }
                    else if (matchup.Player2Id == player.Id)
                    {
                        previousOpponents.Add(allPlayers.Single(p => p.Id == matchup.Player1Id));
                    }
                }
            }

            return previousOpponents.Select(p => new ContestantWithoutPreviouseOpponents
            {
                Identifier = p.Id,
                Affiliation = p.Affiliation,
                Score = allScores.Where(s => s.PlayerId == p.Id).Sum(s => s.Amount)
            }).ToList();
        }
    }

    public class CantGenerateNewRoundException : Exception { }
}
