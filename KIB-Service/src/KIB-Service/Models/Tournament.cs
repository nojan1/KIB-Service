﻿using KIB_Service.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }

        public TournamentDto ToTournamentDto()
        {
            return new TournamentDto
            {
                Id = Id,
                Name = Name,
                Date = Date
            };
        }
    }
}
