using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models
{
    public class Matchup
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int TableNumber { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
    }
}
