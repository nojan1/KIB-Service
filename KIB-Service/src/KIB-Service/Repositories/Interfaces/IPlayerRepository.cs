using KIB_Service.Models;
using KIB_Service.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Player Add(int tournamentId, PlayerDto data);
        ICollection<Player> GetAllInTournament(int tournamentId);
    }
}
