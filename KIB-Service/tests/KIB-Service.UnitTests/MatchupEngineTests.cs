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

    }
}
