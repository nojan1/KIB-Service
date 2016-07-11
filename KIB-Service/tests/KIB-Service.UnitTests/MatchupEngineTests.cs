using KIB_Service.TournamentMatchupEngine;
using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KIB_Service.Tests
{
    public class MatchupEngineTests
    {
        [Fact]
        public void DifferentNumberOfPreviousOpponentsInContestantsShouldThrow()
        {
            var matchupEngine = new MatchupEngine();
            var contestants = new List<Contestant>
            {
                new Contestant
                {
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>
                    {
                        new ContestantWithoutPreviouseOpponents(),
                        new ContestantWithoutPreviouseOpponents(),
                        new ContestantWithoutPreviouseOpponents()
                    }
                },
                new Contestant
                {
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>
                    {
                        new ContestantWithoutPreviouseOpponents(),
                        new ContestantWithoutPreviouseOpponents()
                    }
                }
            };

            Assert.Throws<Exception>(() => { matchupEngine.GenerateMatchup(contestants); });
        }

        [Fact]
        public void GeneratingMatchupsWithoutEnoughConstantsShouldThrow()
        {
            var matchupEngine = new MatchupEngine();
            var contestants = new List<Contestant>
            {
                new Contestant()
            };

            Assert.Throws<Exception>(() => { matchupEngine.GenerateMatchup(contestants); });
        }

        [Fact]
        public void EnteringAnUneveenNumberOfContestantsForRoundOneShouldLeaveOneOut()
        {
            var matchupEngine = new MatchupEngine();
            var contestants = new List<Contestant>
            {
                new Contestant(),
                new Contestant(),
                new Contestant(),
                new Contestant(),
                new Contestant(),
            };

            var matchups = matchupEngine.GenerateMatchup(contestants);
            Assert.Equal(2, matchups.Count);
        }

        [Fact]
        public void GeneratingMatchupsForRound1ByCompShouldReturnCorrectValue()
        {
            var matchupEngine = new MatchupEngine();
            var contestants = new List<Contestant>
            {
                new Contestant
                {
                    Identifier = 1,
                    Score = 0,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>(),
                    CompensationPoints = 18
                },
                new Contestant{
                    Identifier = 2,
                    Score = 0,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>(),
                    CompensationPoints = 20
                },
                new Contestant{
                    Identifier = 3,
                    Score = 0,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>(),
                    CompensationPoints = 16
                },
                new Contestant{
                    Identifier = 4,
                    Score = 0,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>(),
                    CompensationPoints = 17
                },
            };

            var matchups = matchupEngine.GenerateMatchup(contestants);

            Assert.Equal(2, matchups.Count);

            Assert.True((ContestantInMatchup(1, matchups.First()) && ContestantInMatchup(2, matchups.First()) ||
                        (ContestantInMatchup(1, matchups.Last()) && ContestantInMatchup(2, matchups.Last()))));

            Assert.True((ContestantInMatchup(4, matchups.First()) && ContestantInMatchup(3, matchups.First()) ||
                        (ContestantInMatchup(4, matchups.Last()) && ContestantInMatchup(3, matchups.Last()))));
        }

        [Fact]
        public void GeneratingMatchupForRound2ShouldReturnCorrectValue()
        {
            var matchupEngine = new MatchupEngine();
            var contestants = new List<Contestant>
            {
                new Contestant
                {
                    Identifier = 1,
                    Score = 20,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>() { new ContestantWithoutPreviouseOpponents() }
                },
                new Contestant{
                    Identifier = 2,
                    Score = 0,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>() { new ContestantWithoutPreviouseOpponents() }
                },
                new Contestant{
                    Identifier = 3,
                    Score = 5,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>() { new ContestantWithoutPreviouseOpponents() }
                },
                new Contestant{
                    Identifier = 4,
                    Score = 15,
                    PreviousOpponents = new List<ContestantWithoutPreviouseOpponents>() { new ContestantWithoutPreviouseOpponents() }
                },
            };

            var matchups = matchupEngine.GenerateMatchup(contestants);

            Assert.Equal(2, matchups.Count);

            Assert.True((ContestantInMatchup(1, matchups.First()) && ContestantInMatchup(4, matchups.First()) ||
                        (ContestantInMatchup(1, matchups.Last()) && ContestantInMatchup(4, matchups.Last()))));

            Assert.True((ContestantInMatchup(2, matchups.First()) && ContestantInMatchup(3, matchups.First()) ||
                        (ContestantInMatchup(2, matchups.Last()) && ContestantInMatchup(3, matchups.Last()))));
        }

        [Theory]
        [InlineData(20, 5)]
        public void XNumberOfOpponentsInYRandomOutcomeMatchesShouldNotMeatEachOtherTwice(int numPlayers, int numRounds)
        {
            var contestants = Enumerable.Range(1, numPlayers)
                                        .Select(i => new Contestant
                                        {
                                            Identifier = i,
                                            Affiliation = "Same",
                                            Score = 0
                                        })
                                        .ToList();

            var rnd = new Random();
            var matchEngine = new MatchupEngine();

            for (var i = 0; i < numRounds; i++)
            {
                var matchups = matchEngine.GenerateMatchup(contestants);
                foreach(var matchup in matchups)
                {
                    Assert.Null(matchup.Contestant1.PreviousOpponents.FirstOrDefault(c => c.Identifier == matchup.Contestant2.Identifier));
                    Assert.Null(matchup.Contestant2.PreviousOpponents.FirstOrDefault(c => c.Identifier == matchup.Contestant1.Identifier));

                    var contestant1Score = rnd.Next(1, 20);
                    var contestant2Score = 20 - contestant1Score;

                    var contestant1 = contestants.Single(c => c.Identifier == matchup.Contestant1.Identifier);
                    var contestant2 = contestants.Single(c => c.Identifier == matchup.Contestant2.Identifier);

                    contestant1.Score += contestant1Score;
                    contestant1.PreviousOpponents.Add(contestant2);

                    contestant2.Score += contestant2Score;
                    contestant2.PreviousOpponents.Add(contestant1);
                }
            }
        }

        private bool ContestantInMatchup(int identifier, ContestantMatchup matchup)
        {
            return matchup.Contestant1.Identifier == identifier || matchup.Contestant2.Identifier == identifier;
        }
    }
}
