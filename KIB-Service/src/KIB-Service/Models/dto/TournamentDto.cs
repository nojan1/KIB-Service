using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class TournamentDto
    {
        public string Name { get; set; }
        public DateTimeOffset EventDate { get; set; }
    }
}
