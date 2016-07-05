using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KIB_Service.Models.dto;
using KIB_Service.Repositories.Interfaces;

namespace KIB_Service.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private ITournamentRepository tournamentRepository;

        public TournamentController(ITournamentRepository tournamentRepository)
        {
            this.tournamentRepository = tournamentRepository;
        }

        [HttpGet]
        public IEnumerable<TournamentDto> Get()
        {
            return tournamentRepository.List().Select(t => new TournamentDto
            {
                Name = t.Name,
                EventDate = t.Date
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tournament = tournamentRepository.Get(id);
            if(tournament == null)
            {
                return NotFound();
            }

            return Ok(tournament);
        }

        [HttpPost]
        public IActionResult Post([FromBody]TournamentDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var tournament = tournamentRepository.Create(value);

            return Ok(tournament);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
