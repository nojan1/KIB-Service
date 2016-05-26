using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class Matchup
    {
        public Matchup(IEnumerable<Player> players, int table)
        {
            if (players.Count() != 2)
                throw new Exception("Player length must be 2");

            Player1 = players.First();
            Player2 = players.Last();
            Table = table;
        }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int Table { get; set; }
    }
}
