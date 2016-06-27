using KIB_Service.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Controllers.Interfaces
{
    public interface ITournamentRepository
    {
        ICollection<TournamentDto> List();
        TournamentDto Get(int id);
    }
}
