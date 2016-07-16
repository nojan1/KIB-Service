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
            var scoreBoard = allContestants.OrderByDescending(c => c.Score).ToList();
            var contestant1Placement = scoreBoard.FindIndex(c => c.Identifier == contestant1.Identifier);
            var contestant2Placement = scoreBoard.FindIndex(c => c.Identifier == contestant2.Identifier);

            var placementDifference = Math.Abs(contestant1Placement - contestant2Placement);

            return (1.0 / (double)placementDifference) * MaxMatchupScore;
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
