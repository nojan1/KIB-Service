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
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from Tournament where Id=" + id.ToString(); //TODO: Fix!!!!

                conn.Open();
                var reader = cmd.ExecuteReader();
                
            }

            return null;
        }

        public ICollection<Tournament> List()
        {
            return new List<Tournament>();
        }
    }
}
