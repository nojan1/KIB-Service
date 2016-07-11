using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.TournamentMatchupEngine.Models
{
    public class Contestant : ContestantWithoutPreviouseOpponents
    {
        public ICollection<ContestantWithoutPreviouseOpponents> PreviousOpponents { get; set; } = new List<ContestantWithoutPreviouseOpponents>();
    }

    public class ContestantWithoutPreviouseOpponents
    {
        public int Identifier { get; set; }
        public string Affiliation { get; set; }
        public int Score { get; set; }
        public int CompensationPoints { get; set; }
    }
}
