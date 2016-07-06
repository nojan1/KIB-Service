using KIB_Service.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Affiliation { get; set; }
        public bool Active { get; set; }

        public PlayerDto ToPlayerDto()
        {
            return new PlayerDto
            {
                Id = Id,
                Active = Active,
                Name = Name,
                Affiliation = Affiliation
            };
        }
    }
}
