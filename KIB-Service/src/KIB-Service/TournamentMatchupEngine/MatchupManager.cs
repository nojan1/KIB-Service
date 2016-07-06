using KIB_Service.Models;
using KIB_Service.Repositories.Interfaces;
using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine
{
    public class MatchupManager
    {
        private IRoundRepository roundRepository;

        public MatchupManager(IRoundRepository roundRepository)
        {
            this.roundRepository = roundRepository;
        }

        public bool CanGenerateNextRound(int tournamentId)
        {
            var currentRound = roundRepository.GetCurrentRound(tournamentId);
            if (currentRound == null)
                return true;

            //Todo: Make sure that all scores have been set for the current matchups

            return false;
        }

        public Round GenerateMatchups(int tournamentId)
        {
            if (!CanGenerateNextRound(tournamentId))
            {
                throw new CantGenerateNewRoundException();
            }

            var previousMatchups = roundRepository.GetAllMatchups(tournamentId);
            var players = previousMatchups.SelectMany(x => x.SelectMany(y =>
            {
                return new List<Player>
                {
                    y.Player1,
                    y.Player2
                };
            })).Distinct();

            var contestants = players.Select(p => new Contestant
            {
                Identifier = p.Id,
                Affiliation = p.Affiliation,
                PreviousOpponents = ExtractPreviousOpponentsForPlayer(p, previousMatchups),
                Score = 0 //TODO: Calculate score
            }).ToList();

            var engine = new MatchupEngine();
            var contestantMatchups = engine.GenerateMatchup(contestants);

            var round = roundRepository.Add(new Round
            {
                RoundNumber = previousMatchups.Max(x => x.Key) + 1,
                TournamentId = tournamentId
            });

            var matchups = contestantMatchups.Select(x => new Matchup
            {
                RoundId = round.Id,
                Player1Id = x.Contestant1.Identifier,
                Player2Id = x.Contestant2.Identifier
            }).ToList();

            //TODO: Add matchups to DB

            return roundRepository.GetCurrentRound(tournamentId);
        }

        public ICollection<ContestantWithoutPreviouseOpponents> ExtractPreviousOpponentsForPlayer(Player player, IEnumerable<IGrouping<int, Matchup>> allMatchups)
        {
            var previousOpponents = new List<Player>();

            foreach(var roundMatchups in allMatchups)
            {
                foreach(var matchup in roundMatchups)
                {
                    if(matchup.Player1Id == player.Id)
                    {
                        previousOpponents.Add(matchup.Player2);
                    }else if(matchup.Player2Id == player.Id)
                    {
                        previousOpponents.Add(matchup.Player1);
                    }
                }
            }

            return previousOpponents.Select(p => new ContestantWithoutPreviouseOpponents
            {
                Identifier = p.Id,
                Affiliation = p.Affiliation,
                Score = 0 //TODO: Calculate the score for this player
            }).ToList();
        }
    }

    public class CantGenerateNewRoundException : Exception { }
}
