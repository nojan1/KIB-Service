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
            var roundNum = contestants.First().PreviousOpponents.Count + 1;
            if (contestants.Any(c => c.PreviousOpponents.Count != roundNum - 1))
            {
                throw new Exception("Supplied contestants does not have the same number of previous opponents");
            }

            var returnValue = new List<Matchup>();

            if (roundNum != 1)
            {

            }
            else
            {
                //First round, order by affiliation and randomize
                //In theory this should reduce the likelyhood off people from same affiliation meeting each other
 
                var shuffledContestants = contestants.OrderBy(c => c.Affiliation).Shuffle().ToList();
                var tableNumber = 1;

                while(shuffledContestants.Count >= 2)
                {
                    returnValue.Add(new Matchup
                    {
                        Contestant1 = shuffledContestants[0],
                        Contestant2 = shuffledContestants[1],
                        Table = tableNumber++
                    });

                    shuffledContestants.RemoveRange(0, 2);
                }
            }

            return returnValue;
        }
    }
}
