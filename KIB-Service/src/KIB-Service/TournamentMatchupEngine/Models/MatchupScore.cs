﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine.Models
{
    public class MatchupScore
    {
        public ICollection<ContestantMatchup> Matchups { get; set; } = new List<ContestantMatchup>();
        public int Score { get; set; }
    }
}
