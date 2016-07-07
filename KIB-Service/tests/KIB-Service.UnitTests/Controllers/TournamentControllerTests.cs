using KIB_Service.Controllers;
using KIB_Service.Models;
using KIB_Service.Models.dto;
using KIB_Service.Repositories;
using KIB_Service.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections;
using KIB_Service.TournamentMatchupEngine.Interface;
using KIB_Service.TournamentMatchupEngine;

namespace KIB_Service.Tests
{

    public class TournamentControllerTests
    {
        [Fact]
        public void CallingGetShouldAlwaysReturnEnumerableOfTournamentDto()
        {
            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.List())
                          .Returns(new List<Tournament>());

            var ctrl = new TournamentController(mockRepository.Object, null, null, null);

            var result = ctrl.Get();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<TournamentDto>>(result);
        }

        [Fact]
        public void CallingGetOnNonExistingIdShouldReturnNotFound()
        {
            var mockRepository = new Mock<ITournamentRepository>();
            var ctrl = new TournamentController(mockRepository.Object, null, null, null);

            var result = ctrl.Get(-1);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public void CallingGetOnExistingIdShouldReturnOkeyWithCorrectValue()
        {
            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Get(It.Is<int>(x => x == 1)))
                          .Returns(new Tournament());

            var ctrl = new TournamentController(mockRepository.Object, null, null, null);

            var result = ctrl.Get(1);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.NotNull((result as OkObjectResult).Value);
        }

        [Fact]
        public void CallingPostWithInvalidInputDataShouldReturn400()
        {
            var mockRepository = new Mock<ITournamentRepository>();

            var ctrl = new TournamentController(mockRepository.Object, null, null, null);
            ctrl.ModelState.AddModelError("", "error");

            var result = ctrl.CreateTournament(new TournamentDto());

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void CallingPostWithValidDataShouldReturnOkAndObject()
        {
            var model = new TournamentDto
            {
                Name = "Test tournament",
                Date = DateTimeOffset.Now.AddDays(2)
            };

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Add(It.IsAny<TournamentDto>()))
                          .Returns<TournamentDto>((data) => new Tournament { Name = data.Name, Date = data.Date.Value });

            var ctrl = new TournamentController(mockRepository.Object, null, null, null);

            var result = ctrl.CreateTournament(model);

            Assert.IsAssignableFrom<OkObjectResult>(result);

            var tournament = (result as OkObjectResult).Value as TournamentDto;
            Assert.Equal(model.Name, tournament.Name);
            Assert.Equal(model.Date.Value, tournament.Date);
        }

        [Fact]
        public void FetchingMatchupsWhenThereIsNoActiveRoundShouldReturnNoContent()
        {
            var mockRepository = new Mock<IRoundRepository>();
            mockRepository.Setup(r => r.GetAllMatchups(It.IsAny<int>()))
                          .Returns(() => null);

            var ctrl = new TournamentController(null, null, mockRepository.Object, null);

            var result = ctrl.GetMatchups(1);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public void FetchingMatchupsWhenThereIsAnActiveRoundShouldReturnICollectionOfMatchup()
        {
            var mockRepository = new Mock<IRoundRepository>();
            mockRepository.Setup(r => r.GetAllMatchups(It.IsAny<int>()))
                          .Returns(() => new List<IGrouping<int, Matchup>>
                          {
                              new Grouping<int, Matchup>(1, new List<Matchup>
                              {
                                  new Matchup
                                  {
                                      Id = 1,
                                      RoundId= 1,
                                      Player1Id = 1,
                                      Player2Id = 2
                                  }
                              })
                          });

            var ctrl = new TournamentController(null, null, mockRepository.Object, null);

            var result = ctrl.GetMatchups(1);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<RoundMatchupDto>>((result as OkObjectResult).Value);

            var list = (result as OkObjectResult).Value as IEnumerable<RoundMatchupDto>;
            Assert.NotEmpty(list);
        }

        [Fact]
        public void GeneratingMatchupsBeforeRoundIsFinishedShouldReturnBadRequest()
        {
            var matchupManagerMock = new Mock<IMatchupManager>();
            matchupManagerMock.Setup(x => x.GenerateMatchups(It.IsAny<int>()))
                              .Throws<CantGenerateNewRoundException>();

            var ctrl = new TournamentController(null, null, null, matchupManagerMock.Object);
            var result = ctrl.GenerateMatchupsForNextRound(1);

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }


    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly TKey key;
        private readonly IEnumerable<TElement> values;

        public Grouping(TKey key, IEnumerable<TElement> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            this.key = key;
            this.values = values;
        }

        public TKey Key
        {
            get { return key; }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
