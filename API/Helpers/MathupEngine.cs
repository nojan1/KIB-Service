using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    class MathupEngine
    {
        public ICollection<Matchup> GenerateMatchup(int round, Dictionary<int, ICollection<Matchup>> previousMatchups, ICollection<PlayerWithScore> players)
        {
            if (round == 0)
            {
                var matchups = new List<Matchup>();
                var shuffledPlayers = players.Cast<Player>().OrderBy(p => Guid.NewGuid()).ToList();
                int table = 0;

                while (shuffledPlayers.Count() % 2 == 0 && shuffledPlayers.Count() != 0)
                {
                    matchups.Add(new Matchup(shuffledPlayers.Take(2), table++));
                    shuffledPlayers.RemoveRange(0, 2);
                }

                return matchups;
            }
            else
            {

            }

            return new List<Matchup>();
        }



    }
}
