using KIB_Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIB_Service.Models;
using KIB_Service.Models.dto;
using KIB_Service.Helpers;
using System.Data.Common;

namespace KIB_Service.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private DBHelper dbHelper;

        public PlayerRepository(DBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Player Add(int tournamentId, PlayerDto data)
        {
            return dbHelper.Insert("Player", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Name", data.Name),
                new KeyValuePair<string, object>("Affiliation", data.Affiliation),
                new KeyValuePair<string, object>("TournamentId", tournamentId),
                new KeyValuePair<string, object>("Active", true)
            }, UnpackPlayer);
        }

        private Player UnpackPlayer(DbDataReader reader)
        {
            return new Player
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Affiliation = reader.GetString(2),
                Active = reader.GetBoolean(3)
            };
        }
    }
}
