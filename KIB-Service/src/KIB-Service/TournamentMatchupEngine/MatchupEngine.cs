using KIB_Service.Helpers;
using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine
{
    public class MatchupEngine
    {
        public ICollection<Matchup> GenerateMatchup(ICollection<Contestant> contestants)
        {
            if(contestants.Count <= 1)
            {
                throw new Exception("Not enough contestants to generate matchup");
            }

            var roundNum = contestants.First().PreviousOpponents.Count + 1;
            if (contestants.Any(c => c.PreviousOpponents.Count != roundNum - 1))
            {
                throw new Exception("Supplied contestants does not have the same number of previous opponents");
            }

            if (roundNum != 1)
            {
                return MatchupRoundX(contestants);
            }
            else
            {
                return MatchupRound1(contestants);
            }
        }

        private ICollection<Matchup> MatchupRoundX(ICollection<Contestant> contestants)
        {
            var scoredMatchups = new List<MatchupScore>();



            return scoredMatchups.First(m => m.Score == scoredMatchups.Max(x => x.Score)).Matchups;
        }

        private ICollection<Matchup> MatchupRound1(ICollection<Contestant> contestants)
        {
            var returnValue = new List<Matchup>();

            //First round, order by affiliation and randomize
            //In theory this should reduce the likelyhood off people from same affiliation meeting each other

            var shuffledContestants = contestants.OrderBy(c => c.Affiliation).Shuffle().ToList();
            var tableNumber = 1;

            while (shuffledContestants.Count >= 2)
            {
                returnValue.Add(new Matchup
                {
                    Contestant1 = shuffledContestants[0],
                    Contestant2 = shuffledContestants[1],
                    Table = tableNumber++
                });

                shuffledContestants.RemoveRange(0, 2);
            }

            return returnValue;
        }
    }
}
