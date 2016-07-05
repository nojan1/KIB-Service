using KIB_Service.Controllers;
using KIB_Service.Models;
using KIB_Service.Models.dto;
using KIB_Service.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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

            var ctrl = new TournamentController(mockRepository.Object);

            var result = ctrl.Get();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<TournamentDto>>(result);
        }

        [Fact]
        public void CallingGetOnNonExistingIdShouldReturnNotFound()
        {
            var mockRepository = new Mock<ITournamentRepository>();
            var ctrl = new TournamentController(mockRepository.Object);

            var result = ctrl.Get(-1);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public void CallingGetOnExistingIdShouldReturnOkeyWithCorrectValue()
        {
            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Get(It.Is<int>(x => x == 1)))
                          .Returns(new Tournament());

            var ctrl = new TournamentController(mockRepository.Object);

            var result = ctrl.Get(1);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.NotNull((result as OkObjectResult).Value);
        }

        [Fact]
        public void CallingPostWithInvalidInputDataShouldReturn400()
        {
            var mockRepository = new Mock<ITournamentRepository>();

            var ctrl = new TournamentController(mockRepository.Object);
            ctrl.ModelState.AddModelError("", "error");

            var result = ctrl.Post(new TournamentDto());

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void CallingPostWithValidDataShouldReturnOkAndObject()
        {
            var model = new TournamentDto
            {
                Name = "Test tournament",
                EventDate = DateTimeOffset.Now.AddDays(2)
            };

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Create(It.IsAny<TournamentDto>()))
                          .Returns<TournamentDto>((data) => new Tournament { Name = data.Name, Date = data.EventDate.Value });

            var ctrl = new TournamentController(mockRepository.Object);

            var result = ctrl.Post(model);

            Assert.IsAssignableFrom<OkObjectResult>(result);

            var tournament = (result as OkObjectResult).Value as Tournament;
            Assert.Equal(model.Name, tournament.Name);
            Assert.Equal(model.EventDate.Value, tournament.Date);
        }
    }
}
