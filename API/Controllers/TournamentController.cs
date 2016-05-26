using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API
{
    [RoutePrefix("api/Tournament")]
    public class TournamentController : ApiController
    {
        [Route("")]
        public IEnumerable<Tournament> Get()
        {
            return new Tournament[0];
        }

        [Route("{id:int}")]
        public Tournament Get(int id)
        {
            return null;
        }

        [Route("")]
        public void Create([FromBody]Tournament value)
        {

        }

        [Route("{id:int}/GenerateMatchup")]
        public IEnumerable<Matchup> GenerateMatchup(int id)
        {


            return new List<Matchup>();
        }
    }
}