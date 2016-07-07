using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class MatchupDto
    {
        public int TableNumber { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }

        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
    }
}
