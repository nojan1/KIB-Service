using KIB_Service.Models;
using KIB_Service.Repositories.Interfaces;
using KIB_Service.TournamentMatchupEngine;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KIB_Service.Tests.MatchupComponent
{
    public class MatchupManagerTests
    {
        [Fact]
        public void UnevenNumberOfPlayerShouldCreatePseudoRound()
        {
            var roundRepository = new Mock<IRoundRepository>();
            roundRepository.Setup(r => r.GetCurrentRound(It.IsAny<int>()))
                           .Returns(() => null);

            roundRepository.Setup(r => r.GetScoresForTournament(It.IsAny<int>()))
                           .Returns(new List<Score>());

            roundRepository.Setup(r => r.Add(It.IsAny<Round>()))
                           .Returns<Round>(round => new Round
                           {
                               Id = 1,
                               Matchups = round.Matchups,
                               RoundNumber = round.RoundNumber,
                               TournamentId = round.TournamentId
                           });

            var playerRepository = new Mock<IPlayerRepository>();
            playerRepository.Setup(r => r.GetAllInTournament(It.IsAny<int>()))
                            .Returns(new List<Player>
                            {
                                new Player
                                {
                                    Id = 1,
                                    Name = "Player 1",
                                    Active = true
                                },
                                new Player
                                {
                                    Id = 2,
                                    Name = "Player 2",
                                    Active = true
                                },
                                new Player
                                {
                                    Id = 3,
                                    Name = "Player 3",
                                    Active = true
                                }
                            });

            var matchupManager = new MatchupManager(roundRepository.Object, playerRepository.Object);

            matchupManager.GenerateMatchups(1);

            roundRepository.Verify(r => r.AddPseudoMatchupToRound(1, It.IsAny<int>()));
        }

        [Fact]
        public void NotEnoughActivePlayersShouldThrow()
        {
            var roundRepository = new Mock<IRoundRepository>();
            roundRepository.Setup(r => r.GetCurrentRound(It.IsAny<int>()))
                           .Returns(() => null);

            roundRepository.Setup(r => r.GetScoresForTournament(It.IsAny<int>()))
                           .Returns(new List<Score>());

            roundRepository.Setup(r => r.Add(It.IsAny<Round>()))
                           .Returns<Round>(round => new Round
                           {
                               Id = 1,
                               Matchups = round.Matchups,
                               RoundNumber = round.RoundNumber,
                               TournamentId = round.TournamentId
                           });

            var playerRepository = new Mock<IPlayerRepository>();
            playerRepository.Setup(r => r.GetAllInTournament(It.IsAny<int>()))
                            .Returns(new List<Player>
                            {
                                new Player
                                {
                                    Id = 1,
                                    Name = "Player 1",
                                    Active = false
                                },
                                new Player
                                {
                                    Id = 2,
                                    Name = "Player 2",
                                    Active = false
                                },
                                new Player
                                {
                                    Id = 3,
                                    Name = "Player 3",
                                    Active = true
                                }
                            });

            var matchupManager = new MatchupManager(roundRepository.Object, playerRepository.Object);

            Assert.Throws<Exception>(() => { matchupManager.GenerateMatchups(1); });
        }
    }
}
