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

        public TournamentRepository(DbConnection conn)
        {
            this.conn = conn;
        }

        public Tournament Create(TournamentDto data)
        {
            var arguments = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Name", data.Name),
                new KeyValuePair<string, object>("Date", data.EventDate)
            };

            return conn.Insert("Tournament", arguments, UnpackTournament);
        }

        public Tournament Get(int id)
        {
            return conn.Get("select Id, Name, Date from Tournament where Id=" + id.ToString() + " limit 1", UnpackTournament);
        }

        public ICollection<Tournament> List()
        {
            return conn.Query("select Id, Name, Date from Tournament", UnpackTournament);
        }

        private Tournament UnpackTournament(DbDataReader reader)
        {
            return new Tournament
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Date = reader.GetDateTime(2)
            };
        }
    }
}
