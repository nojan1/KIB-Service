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
        private DBHelper dbHelper;

        public TournamentRepository(DBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Tournament Add(TournamentDto data)
        {
            var arguments = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Name", data.Name),
                new KeyValuePair<string, object>("Date", data.EventDate.Value.ToString())
            };

            return dbHelper.Insert("Tournament", arguments, UnpackTournament, "Id, Name, Date");
        }

        public Tournament Get(int id)
        {
            return dbHelper.Get("select Id, Name, Date from Tournament where Id=" + id.ToString() + " limit 1", UnpackTournament);
        }

        public ICollection<Tournament> List()
        {
            return dbHelper.Query("select Id, Name, Date from Tournament", UnpackTournament);
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
