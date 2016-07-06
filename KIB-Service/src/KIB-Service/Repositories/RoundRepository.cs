using KIB_Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIB_Service.Models;
using KIB_Service.Helpers;
using System.Data.Common;

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

        public IEnumerable<IGrouping<int, Matchup>> GetAllMatchups(int tournamentId)
        {
            var matchups = dbHelper.Query(@"select m.Id, m.Player1Id, m.Player2Id, m.RoundId, r.RoundNumber from Matchup as m 
                                            inner join Round as r on r.Id = m.RoundId
                                            where r.TournamentId = " + tournamentId, 
                           (reader) =>
                           {
                               return new
                               {
                                   RoundNumber = reader.GetInt32(4),
                                   Matchup = UnpackMatchup(reader)
                               };
                           });

            return matchups.GroupBy(x => x.RoundNumber, 
                                    x => x.Matchup);
        }

        public Round GetCurrentRound(int tournamentId)
        {
            var round = dbHelper.Get(@"select Id, RoundNumber, TournamentId from Round where TournamentId = " + tournamentId + " and RoundNumber = (select max(RoundNumber) from Round where TournamentId = " + tournamentId + ")", UnpackRound);

            if (round == null)
                return null;

            round.Matchups = dbHelper.Query(@"select Id, Player1Id, Player2Id, RoundId from Matchup where RoundId = " + round.Id, UnpackMatchup);

            return round;
        }

        public void SetScore(int matchupId, int player1Score, int player2Score)
        {
            throw new NotImplementedException();
        }

        private Matchup UnpackMatchup(DbDataReader reader)
        {
            return new Matchup
            {
                Id = reader.GetInt32(0),
                RoundId = reader.GetInt32(3),
                Player1Id = reader.GetInt32(1),
                Player2Id = reader.GetInt32(2)
            };
        }

        private Round UnpackRound(DbDataReader reader)
        {
            return new Round
            {
                Id = reader.GetInt32(0),
                RoundNumber = reader.GetInt32(1),
                TournamentId = reader.GetInt32(2)
            };
        }
    }
}
