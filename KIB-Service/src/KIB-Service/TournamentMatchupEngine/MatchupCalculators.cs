using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine
{
    public class MatchupCalculators
    {
        public static double HaveMetBefore(Contestant contestant1, Contestant contestant2, ICollection<Contestant> allContestants)
        {
            if (contestant1.PreviousOpponents.Any(o => o.Identifier == contestant2.Identifier) ||
                  contestant2.PreviousOpponents.Any(o => o.Identifier == contestant1.Identifier))
            {
                return (MaxMatchupScore + 2) * -1;
            }

            return 1;
        }

        private const double MaxMatchupScore = 4;
        public static double ClosestInScore(Contestant contestant1, Contestant contestant2, ICollection<Contestant> allContestants)
        {
            int scoreDifference = Math.Abs(contestant1.Score - contestant2.Score);
            var bestScoreDifference = allContestants.Where(c => c.Identifier != contestant1.Identifier).Select(c => Math.Abs(c.Score - contestant1.Score)).Min();

            if (scoreDifference == 0)
                return MaxMatchupScore;

            return ((double)bestScoreDifference / (double)scoreDifference) * MaxMatchupScore;
        }

        public static double SameAffiliation(Contestant contestant1, Contestant contestant2, ICollection<Contestant> allContestants)
        {
            if(allContestants.All(c => c.Affiliation == allContestants.First().Affiliation))
            {
                return 0;
            }

            if (contestant1.Affiliation == contestant2.Affiliation)
            {
                return -1;
            }

            return 0;
        }
    }
}
