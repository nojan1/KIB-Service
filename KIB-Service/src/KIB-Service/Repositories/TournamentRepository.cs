using KIB_Service.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIB_Service.Models.dto;

namespace KIB_Service.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        public TournamentDto Get(int id)
        {
            return null;
        }

        public ICollection<TournamentDto> List()
        {
            return new List<TournamentDto>();
        }
    }
}
