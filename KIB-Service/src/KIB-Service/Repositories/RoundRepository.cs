using KIB_Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIB_Service.Models;
using KIB_Service.Helpers;

namespace KIB_Service.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private DBHelper dbHelper;

        public RoundRepository(DBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public void Add(Round newRound)
        {
            throw new NotImplementedException();
        }

        public Round GetCurrentRound(int tournamentId)
        {
            var round = dbHelper.Get(@"select Id, RoundNumber from Round where TournamentId = " + tournamentId + " and RoundNumber = (select max(RoundNumber) from Round where TournamentId = " + tournamentId + ")", 
                
                (reader) =>
            {
                return new Round
                {
                    Id = reader.GetInt32(0),
                    RoundNumber = reader.GetInt32(1),
                    TournamentId = tournamentId
                };
            });

            if (round == null)
                return null;

            round.Matchups = dbHelper.Query(@"select Id, Player1Id, Player2Id from Matchup where RoundId = " + round.Id, (reader) =>
            {
                return new Matchup
                {
                    Id = reader.GetInt32(0),
                    RoundId = round.Id,
                    Player1Id = reader.GetInt32(1),
                    Player2Id = reader.GetInt32(2)
                };
            });

            return round;
        }

        public void SetScore(int matchupId, int player1Score, int player2Score)
        {
            throw new NotImplementedException();
        }
    }
}
