using KIB_Service.Models;
using KIB_Service.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Repositories.Interfaces
{
    public interface ITournamentRepository
    {
        ICollection<Tournament> List();
        Tournament Get(int id);
        Tournament Create(TournamentDto data);
    }
}
