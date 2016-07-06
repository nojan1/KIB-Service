using KIB_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine
{
    public class MatchupManager
    {

        public MatchupManager()
        {

        }

        public bool CanGenerateNextRound()
        {
            return false;
        }

        public Round GenerateMatchups()
        {


            return new Round();
        }
    }
}
