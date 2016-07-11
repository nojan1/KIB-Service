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
        public ICollection<ContestantMatchup> GenerateMatchup(ICollection<Contestant> contestants)
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
        
        private ICollection<ContestantMatchup> MatchupRound1(ICollection<Contestant> contestants)
        {
            var returnValue = new List<ContestantMatchup>();

            //First round, sort by CompensationPoints and pick groups.
            //OR if all 0 order by affiliation and randomize
            //In theory this should reduce the likelyhood off people from same affiliation meeting each other

            var orderedContestants = contestants.All(c => c.CompensationPoints == 0) ? contestants.OrderBy(c => c.Affiliation).Shuffle().ToList()
                                                                                     : contestants.OrderByDescending(c => c.CompensationPoints).ToList();
            var tableNumber = 1;

            while (orderedContestants.Count >= 2)
            {
                returnValue.Add(new ContestantMatchup
                {
                    Contestant1 = orderedContestants[0],
                    Contestant2 = orderedContestants[1],
                    Table = tableNumber++
                });

                orderedContestants.RemoveRange(0, 2);
            }

            return returnValue;
        }

        private ICollection<ContestantMatchup> MatchupRoundX(ICollection<Contestant> contestants)
        {
            var scoredMatchups = new List<MatchupScore>();

            var allPossibleMatchups = GenerateAllPossibleMatchups(contestants);
            foreach(var matchups in allPossibleMatchups)
            {
                scoredMatchups.Add(new MatchupScore
                {
                    Score = CalculateScoreForMatchup(matchups, contestants),
                    Matchups = matchups.ToList()
                });
            }

            var bestMatchup = scoredMatchups.First(m => m.Score == scoredMatchups.Max(x => x.Score)).Matchups.ToList().Shuffle().ToList();
            for (int i = 1; i <= bestMatchup.Count; i++)
                bestMatchup[i - 1].Table = i;

            return bestMatchup;
        }

        private int CalculateScoreForMatchup(IEnumerable<ContestantMatchup> matchups, ICollection<Contestant> allContestants)
        {
            int score = 0;

            foreach(var matchup in matchups)
            {
                //If it is the best score match
                int scoreDifference = Math.Abs(matchup.Contestant1.Score - matchup.Contestant2.Score);
                var contest1ScoreDifference = allContestants.Where(c => c.Identifier != matchup.Contestant1.Identifier).Select(c => Math.Abs(c.Score - matchup.Contestant1.Score)).Min();

                if(scoreDifference == contest1ScoreDifference)
                {
                    score += 1;
                }

                //If the contestants have met before
                if (matchup.Contestant1.PreviousOpponents.Any(o => o.Identifier == matchup.Contestant2.Identifier) ||
                   matchup.Contestant2.PreviousOpponents.Any(o => o.Identifier == matchup.Contestant1.Identifier))
                {
                    score -= 2;
                }

                //If the contestants have the same affiliation give minus points
                if(matchup.Contestant1.Affiliation == matchup.Contestant2.Affiliation)
                {
                    score -= 1;
                }
            }

            return score;
        }

        private IEnumerable<IEnumerable<ContestantMatchup>> GenerateAllPossibleMatchups(ICollection<Contestant> contestants)
        {
            //var allMatchups = (from m in Enumerable.Range(0, 1 << contestants.Count)
            //        select
            //            from i in Enumerable.Range(0, contestants.Count)
            //            where (m & (1 << i)) != 0
            //            select contestants.ElementAt(i))
            //        .Where(x => x.Count() == 2)
            //        .Select(x => new ContestantMatchup
            //        {
            //            Contestant1 = x.ElementAt(0),
            //            Contestant2 = x.ElementAt(1)
            //        })
            //        .ToList();


            var allMatchups = FastPowerSet(contestants)
                                .Where(x => x.Count() == 2)
                                .Select(x => new ContestantMatchup
                                {
                                    Contestant1 = x.ElementAt(0),
                                    Contestant2 = x.ElementAt(1)
                                })
                                .ToList();

            var returnValue = new List<IEnumerable<ContestantMatchup>>();
            while (allMatchups.Any())
            {
                var workingSet = new List<ContestantMatchup>();
                foreach(var contestant in contestants)
                {
                    if(!workingSet.Any(m => m.Contestant1.Identifier == contestant.Identifier || m.Contestant2.Identifier == contestant.Identifier))
                    {
                        var matchup = allMatchups.First(m => (m.Contestant1.Identifier == contestant.Identifier || m.Contestant2.Identifier == contestant.Identifier) &&
                                                              !workingSet.Any(m2 => m2.Contestant1.Identifier == m.Contestant1.Identifier || m2.Contestant1.Identifier == m.Contestant2.Identifier ||
                                                                                    m2.Contestant2.Identifier == m.Contestant1.Identifier || m2.Contestant2.Identifier == m.Contestant2.Identifier));

                        allMatchups.Remove(matchup);
                        workingSet.Add(matchup);
                    }

                    if (!allMatchups.Any())
                        break;
                }

                returnValue.Add(workingSet);
            }

            return returnValue;
        }

        private IEnumerable<IEnumerable<T>> FastPowerSet<T>(IEnumerable<T> seq)
        {
            var powerSet = new T[1 << seq.Count()][];
            powerSet[0] = new T[0]; // starting only with empty set
            for (int i = 0; i < seq.Count(); i++)
            {
                var cur = seq.ElementAt(i);
                int count = 1 << i; // doubling list each time
                for (int j = 0; j < count; j++)
                {
                    var source = powerSet[j];
                    var destination = powerSet[count + j] = new T[source.Length + 1];
                    for (int q = 0; q < source.Length; q++)
                        destination[q] = source[q];
                    destination[source.Length] = cur;
                }
            }
            return powerSet;
        }
    }
}
