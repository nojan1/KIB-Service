using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIB_Service.Models.dto;
using KIB_Service.Repositories.Interfaces;
using KIB_Service.Models;
using KIB_Service.Helpers;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace KIB_Service.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private DbConnection conn;

        public TournamentRepository(ConnectionStringOption connectionStringOption)
        {
            conn = new MySqlConnection(connectionStringOption.ConnectionString);
        }

        public void Add(Tournament tournament)
        {
            throw new NotImplementedException();
        }

        public Tournament Get(int id)
        {
            return getTournaments(" where Id=" + id.ToString() + " limit 1").FirstOrDefault();
        }

        public ICollection<Tournament> List()
        {
            return getTournaments("");
        }

        private ICollection<Tournament> getTournaments(string sqlPostfix)
        {
            var tournaments = new List<Tournament>();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select Id, Name, Date from Tournament" + sqlPostfix;

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tournaments.Add(new Tournament
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Date = reader.GetDateTime(2)
                    });
                }
            }

            return tournaments;
        }
    }
}
