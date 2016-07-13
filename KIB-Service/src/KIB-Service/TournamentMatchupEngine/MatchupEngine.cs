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
            if (contestants.Count <= 1)
            {
                throw new Exception("Not enough contestants to generate matchup");
            }

            if(contestants.Count % 2 != 0)
            {
                throw new Exception("Number of contestants must be even");
            }

            var roundNum = contestants.First().PreviousOpponents.Count + 1;

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
            var allContestantMatchups = ScoreContestantMatchups(contestants, MatchupCalculators.HaveMetBefore, MatchupCalculators.SameAffiliation, MatchupCalculators.ClosestInScore);
            var scoredMatchups = CombineAndScoreContestantMatchups(allContestantMatchups, contestants.OrderByDescending(c => c.Score).ToList());

            var bestMatchup = scoredMatchups.First(m => m.Score == scoredMatchups.Max(x => x.Score)).Matchups.ToList().Shuffle().ToList();
            for (int i = 1; i <= bestMatchup.Count; i++)
                bestMatchup[i - 1].Table = i;

            return bestMatchup;
        }

        private ICollection<MatchupScore> CombineAndScoreContestantMatchups(ICollection<ScoredContestantMatchup> allContestantMatchups, ICollection<Contestant> allContestants)
        {
            var returnValue = new List<MatchupScore>();

            for(var i = 0; i < allContestants.Count; i++)
            {
                var contestants = allContestants.Skip(i).ToList();
                contestants.AddRange(allContestants.Take(i));

                var matchup = new MatchupScore();

                foreach (var contestant in contestants)
                {
                    var scoredMatchup = allContestantMatchups.FirstOrDefault(m => (m.Contestant1.Identifier == contestant.Identifier || m.Contestant2.Identifier == contestant.Identifier) &&
                                                                                  !matchup.Matchups.Any(x => x.Contestant1.Identifier == m.Contestant1.Identifier ||
                                                                                                             x.Contestant1.Identifier == m.Contestant2.Identifier ||
                                                                                                             x.Contestant2.Identifier == m.Contestant1.Identifier ||
                                                                                                             x.Contestant2.Identifier == m.Contestant2.Identifier));
                    if (scoredMatchup == null)
                    {
                        continue;
                    }

                    matchup.Matchups.Add(new ContestantMatchup
                    {
                        Contestant1 = scoredMatchup.Contestant1,
                        Contestant2 = scoredMatchup.Contestant2
                    });

                    matchup.Score += scoredMatchup.Score;
                }

                returnValue.Add(matchup);
            }

            return returnValue;
        }

        private ICollection<ScoredContestantMatchup> ScoreContestantMatchups(ICollection<Contestant> contestants, params Func<Contestant, Contestant, ICollection<Contestant>, int>[] calculators)
        {
            var retval = new List<ScoredContestantMatchup>();

            foreach (var contestant1 in contestants)
            {
                foreach (var contestant2 in contestants)
                {
                    if (contestant1 == contestant2 || retval.Any(c => (c.Contestant1.Identifier == contestant1.Identifier || c.Contestant1.Identifier == contestant2.Identifier) &&
                                                                      (c.Contestant2.Identifier == contestant1.Identifier || c.Contestant2.Identifier == contestant2.Identifier)))
                    {
                        continue;
                    }

                    retval.Add(new ScoredContestantMatchup
                    {
                        Contestant1 = contestant1,
                        Contestant2 = contestant2,
                        Score = calculators.Sum(c => c.Invoke(contestant1, contestant2, contestants))
                    });
                }
            }

            return retval.OrderByDescending(x => x.Score).ToList();
        }
    }
}
