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

        public Round Add(Round newRound)
        {
            return dbHelper.Insert("Round",
                                   new List<KeyValuePair<string, object>>
                                   {
                                       new KeyValuePair<string, object>("RoundNumber", newRound.RoundNumber),
                                       new KeyValuePair<string, object>("TournamentId", newRound.TournamentId)
                                   },
                                   UnpackRound,
                                   "Id, RoundNumber, TournamentId");
        }

        public void AddMatchupsToRound(int roundId, ICollection<Matchup> matchups)
        {
            foreach(var matchup in matchups)
            {
                dbHelper.Insert("Matchup",
                                new List<KeyValuePair<string, object>>
                                {
                                    new KeyValuePair<string, object>("RoundId", roundId),
                                    new KeyValuePair<string, object>("TableNumber", matchup.TableNumber),
                                    new KeyValuePair<string, object>("Player1Id", matchup.Player1Id),
                                    new KeyValuePair<string, object>("Player2Id", matchup.Player2Id)
                                });
            }
        }

        public IEnumerable<IGrouping<int, Matchup>> GetAllMatchups(int tournamentId)
        {
            var matchups = dbHelper.Query(@"select m.Id, m.Player1Id, m.Player2Id, m.RoundId, m.TableNumber, r.RoundNumber from Matchup as m 
                                            inner join Round as r on r.Id = m.RoundId
                                            where r.TournamentId = " + tournamentId, 
                           (reader) =>
                           {
                               return new
                               {
                                   RoundNumber = reader.GetInt32(5),
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

            round.Matchups = dbHelper.Query(@"select Id, Player1Id, Player2Id, RoundId, TableNumber from Matchup where RoundId = " + round.Id, UnpackMatchup);

            return round;
        }

        public ICollection<Score> GetScoresForTournament(int tournamentId)
        {
            return dbHelper.Query(@"select s.Id, s.MatchupId, s.Amount, s.PlayerId from Score as s
                                    inner join Matchup as m on s.MatchupId = m.Id
                                    inner join Round as r on m.RoundId = r.Id
                                    where r.TournamentId = " + tournamentId, 
                (reader) =>
            {
                return new Score
                {
                    Id = reader.GetInt32(0),
                    MatchupId = reader.GetInt32(1),
                    Amount = reader.GetInt32(2),
                    PlayerId = reader.GetInt32(3)
                };
            });
        }

        public void SetScore(int matchupId, int player1Score, int player2Score)
        {
            var updateScore = new Action<int, int>((playerId, amount) =>
            {
                var existingScore = dbHelper.Get("select Id, MatchupId, PlayerId, Amount from Score where MatchupId = " + matchupId + " and PlayerId = " + playerId,
                (reader) =>
                {
                    return new Score
                    {
                        Id = reader.GetInt32(0),
                        MatchupId = reader.GetInt32(1),
                        PlayerId = reader.GetInt32(2),
                        Amount = reader.GetInt32(3)
                    };
                });

                if(existingScore == null)
                {
                    dbHelper.Insert("Score",
                                    new List<KeyValuePair<string, object>>
                                    {
                                        new KeyValuePair<string, object>("MatchupId", matchupId),
                                        new KeyValuePair<string, object>("PlayerId", playerId),
                                        new KeyValuePair<string, object>("Amount", amount)
                                    });
                }
                else
                {
                    dbHelper.Update("Score",
                                    new List<KeyValuePair<string, object>>
                                    {
                                        new KeyValuePair<string, object>("Amount", amount)
                                    },
                                    new KeyValuePair<string, object>("Id", existingScore.Id));
                }
            });

            var matchup = dbHelper.Get("select Id, Player2Id, Player1Id, RoundId, TableNumber from matchup where Id = " + matchupId, UnpackMatchup);
            if(matchup == null)
            {
                throw new Exception("No such matchup!");
            }

            updateScore(matchup.Player1Id, player1Score);
            updateScore(matchup.Player2Id, player2Score);
        }

        private Matchup UnpackMatchup(DbDataReader reader)
        {
            return new Matchup
            {
                Id = reader.GetInt32(0),
                Player1Id = reader.GetInt32(1),
                Player2Id = reader.GetInt32(2),
                RoundId = reader.GetInt32(3),
                TableNumber = reader.GetInt32(4)
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
