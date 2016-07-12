using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine
{
    public class MatchupCalculators
    {
        public static int HaveMetBefore(Contestant contestant1, Contestant contestant2, ICollection<Contestant> allContestants)
        {
            if (contestant1.PreviousOpponents.Any(o => o.Identifier == contestant2.Identifier) ||
                  contestant2.PreviousOpponents.Any(o => o.Identifier == contestant1.Identifier))
            {
                return -2;
            }

            return 0;
        }

        public static int ClosestInScore(Contestant contestant1, Contestant contestant2, ICollection<Contestant> allContestants)
        {
            int scoreDifference = Math.Abs(contestant1.Score - contestant2.Score);
            var contest1ScoreDifference = allContestants.Where(c => c.Identifier != contestant1.Identifier).Select(c => Math.Abs(c.Score -contestant1.Score)).Min();

            if (scoreDifference == contest1ScoreDifference)
            {
                return 1;
            }

            return 0;
        }

        public static int SameAffiliation(Contestant contestant1, Contestant contestant2, ICollection<Contestant> allContestants)
        {
            if (contestant1.Affiliation == contestant2.Affiliation)
            {
                return -1;
            }

            return 0;
        }
    }
}
