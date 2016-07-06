using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class MatchupDto
    {
        public PlayerDto Player1 { get; set; }
        public PlayerDto Player2 { get; set; }

        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
    }
}
