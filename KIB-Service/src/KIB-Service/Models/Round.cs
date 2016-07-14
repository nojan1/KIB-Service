using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models
{
    public class Round
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }

        public int RoundNumber { get; set; }

        public bool Public { get; set; }

        public ICollection<Matchup> Matchups { get; set; }
    }
}
