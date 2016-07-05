using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class TournamentDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTimeOffset? EventDate { get; set; }
    }
}
