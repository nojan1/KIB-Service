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

        private bool ContestantInMatchup(int identifier, ContestantMatchup matchup)
        {
            return matchup.Contestant1.Identifier == identifier || matchup.Contestant2.Identifier == identifier;
        }
    }
}
