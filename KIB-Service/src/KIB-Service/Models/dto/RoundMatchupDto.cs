using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class RoundMatchupDto
    {
        public int RoundNumber { get; set; }
        public ICollection<MatchupDto> Matchups { get; set; }
    }
}
