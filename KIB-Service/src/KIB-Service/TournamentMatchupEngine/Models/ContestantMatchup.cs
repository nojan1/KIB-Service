﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine.Models
{
    public class ContestantMatchup
    {
        public Contestant Contestant1 { get; set; }
        public Contestant Contestant2 { get; set; }
        public int Table { get; set; }
    }

    public class ScoredContestantMatchup : ContestantMatchup
    {
        public double Score { get; set; }
    }
}
