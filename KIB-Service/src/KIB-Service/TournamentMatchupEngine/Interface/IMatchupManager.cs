using KIB_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine.Interface
{
    public interface IMatchupManager
    {
        bool CanGenerateNextRound(int tournamentId);
        Round GenerateMatchups(int tournamentId);
    }
}
