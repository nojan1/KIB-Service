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

namespace KIB_Service.Tests.Controllers
{
    public class TournamentControllerPlayerTests
    {
        [Fact]
        public void CallingAddPlayerWithValidDataShouldReturnOkAndObject()
        {
            var model = new PlayerDto
            {
                Name = "Test player",
                Affiliation = "umbrella"
            };

            var mockRepository = new Mock<IPlayerRepository>();
            mockRepository.Setup(r => r.Add(It.IsAny<int>(), It.IsAny<PlayerDto>()))
                          .Returns<int, PlayerDto>((tournamentId, data) => new Player { Name = data.Name, Affiliation = data.Affiliation, Active = true });

            var ctrl = new TournamentController(null, mockRepository.Object);

            var result = ctrl.AddPlayer(1, model);

            Assert.IsAssignableFrom<OkObjectResult>(result);

            var player = (result as OkObjectResult).Value as PlayerDto;
            Assert.Equal(model.Name, player.Name);
            Assert.Equal(model.Affiliation, player.Affiliation);
            Assert.True(player.Active);
        }

        [Fact]
        public void CallingAddPlayerWithInvalidDataShouldReturBadRequest()
        {
            var ctrl = new TournamentController(null, null);
            ctrl.ModelState.AddModelError("", "Error!");

            var result = ctrl.AddPlayer(1, null);

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }
}
