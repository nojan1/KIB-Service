using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class Player
    {
        public string Name { get; set; }
    }

    public class PlayerWithScore : Player
    {
        public int Score { get; set; }
    }
}
