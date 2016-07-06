using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int MatchupId { get; set; }
        public int Amount { get; set; }

        public Matchup Matchup { get; set; }
    }
}
            